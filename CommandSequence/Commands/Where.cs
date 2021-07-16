using CommandSequence.Commands.Base;
using ClassificationTree.Infraestructure.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace CommandSequence.Commands
{
    public class Where :IInterpreter
    {

        IInterpreter _command;
        IList<(IFunction.function, string)> _seq;
        public Where(IInterpreter command, IList<(IFunction.function, string)> seq) {
            _command = command;
            _seq = seq;
            OrderSequence();
        }

        public DataTable Execute() {

            

            List<string> whereClause = new List<string>();
            List<object> param = new List<object>();
            string lastConjution = string.Empty;
            bool exitLoop = false;
            for (int i = 0; i < _seq.Count; i++) {
                switch (_seq[i].Item1) {
                    case IFunction.function.Operator:
                        string value_oper = whereClause[whereClause.Count - 1];
                        if (value_oper == "<" || value_oper == ">" || value_oper == "=") {
                            whereClause.Insert(whereClause.Count - 1, $"{value_oper}{_seq[i].Item2}");
                            whereClause.RemoveAt(whereClause.Count - 1);
                        } else {
                            whereClause.Add(_seq[i].Item2);
                        }
                        break;
                    case IFunction.function.Conjunction:
                        var valueUp = _seq.ElementAtOrDefault(i + 1);
                        var valueDown = _seq.ElementAtOrDefault(i - 1);
                        if (valueUp.Item1 == IFunction.function.Operator && valueDown.Item1 == IFunction.function.Operator) {
                        } else {
                            whereClause.Add(_seq[i].Item2);
                            lastConjution = _seq[i].Item2;
                        }
                        break;
                    case IFunction.function.TableField:
                        whereClause.Add($"Convert.ToString(it[\"{_seq[i].Item2}\"]).ToLower()");
                        var valueUpTF = _seq.ElementAtOrDefault(i + 1);
                        var valueDownTF = _seq.ElementAtOrDefault(i - 1);
                        if (!(valueUpTF.Item1 != IFunction.function.Operator ^ valueDownTF.Item1 != IFunction.function.Operator)) {
                            whereClause.Add("=");
                        }
                        //whereClause = $"{whereClause} Convert.ToString(it[\"{_seq[i].Item2}\"]).ToLower().Contains(@{param.Count-1},StringComparison.InvariantCultureIgnoreCase)";
                        break;
                    case IFunction.function.Filter:
                        param.Add(Convert.ToString(_seq[i].Item2)?.ToLower());
                        whereClause.Add($"@{param.Count - 1}");
                        break;
                    case IFunction.function.Punct:
                        whereClause.Add($"{lastConjution}");
                        break;
                    case IFunction.function.TableSearchField:
                        exitLoop = true;
                        break;
                    case IFunction.function.None:
                    default:
                        break;
                }
                if (exitLoop) break;

            }
            string value = String.Join(" ", whereClause);
            IQueryable<DataRow> dataRows = _command.Execute().AsEnumerable().AsQueryable().Where(String.Join(" ", whereClause), param.ToArray());


            if (dataRows.Any()) {
                return dataRows?.CopyToDataTable();
            } else {
                return new DataTable();
            }
        }


        private void OrderSequence() {
            List<(IFunction.function, string)> disorderlyList = _seq.ToList();
            List<(IFunction.function, string)> orderList = new List<(IFunction.function, string)>();
            var indexSplit = disorderlyList.Select((x, i) => (x.Item1 == IFunction.function.Conjunction || x.Item1 == IFunction.function.Punct) ? i : -1).Where(i => i != -1).ToList();
            indexSplit.Add(disorderlyList.Count);

            int index = 0;
            foreach (var item in indexSplit) {
                var partialListOrder = disorderlyList.GetRange(index, item - index);
                orderList.AddRange(partialListOrder.Where(x => x.Item1 == IFunction.function.TableField));
                orderList.AddRange(partialListOrder.Where(x => x.Item1 == IFunction.function.Operator));
                orderList.AddRange(partialListOrder.Where(x => x.Item1 == IFunction.function.Filter));
                if (item < disorderlyList.Count) {
                    orderList.Add(disorderlyList[item]);
                }
                index = item + 1;
            }
            _seq = orderList;
        }
    }
}
