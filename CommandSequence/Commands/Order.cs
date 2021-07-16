using CommandSequence.Commands.Base;
using ClassificationTree.Infraestructure.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;

namespace CommandSequence.Commands
{
    class Order :IInterpreter
    {
        IInterpreter _command;
        IList<(IFunction.function, string)> _seq;
        public Order(IInterpreter command, IList<(IFunction.function, string)> seq) {
            _command = command;
            _seq = seq;
        }
        public DataTable Execute() {

            List<string> orderClause = new List<string>();
            for (int i = 0; i < _seq.Count; i++) {
                var item = _seq[i];
                switch (item.Item1) {
                    case IFunction.function.Conjunction: 
                    case IFunction.function.Punct:
                        orderClause.Add(",");
                        break;
                    default:
                    case IFunction.function.Order:
                        orderClause.Add(item.Item2);
                        break;
                    case IFunction.function.TableOrderField:
                        orderClause.Insert(orderClause.Count-1, item.Item2);                            
                        break;
                    case IFunction.function.TableField:
                    case IFunction.function.None:
                    case IFunction.function.TableSearchField:
                    case IFunction.function.Filter:
                    case IFunction.function.Operator:
                        break;
                }
            }             
            var partialResult = _command.Execute();
            if (partialResult.Rows.Count > 0) {
                try {
                    partialResult = partialResult?.AsEnumerable().AsQueryable()?.OrderBy(String.Join(" ", orderClause))?.CopyToDataTable();
                } catch (Exception ex) {  
                    //Case if you try to sort by a column that does not exist. 
                    //returns the data without ordening
                    return partialResult;
                }
            }
            return partialResult;
        }
    }
}
