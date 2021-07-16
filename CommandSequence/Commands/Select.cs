using ClassificationTree.Infraestructure.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CommandSequence.Commands.Base
{
    public class Select :IInterpreter
    {
        IInterpreter _command;
        IList<(IFunction.function, string)> _seq;
        public Select(IInterpreter command, IList<(IFunction.function, string)> seq) {
            _command = command;
            _seq = seq;
        }

        public DataTable Execute() {

            DataTable dt = new DataTable();
            var dataRows = _command.Execute();
            if (dataRows.Rows.Count == 0) return dataRows;
            for (int i = 0; i < _seq.Count; ++i) {
                if (_seq[i].Item1 == IFunction.function.TableSearchField) {
                    dt.Columns.Add(_seq[i].Item2, dataRows.Columns[_seq[i].Item2].DataType);
                }
            }
            var y = ((IEnumerable<DataRow>)dataRows?.AsEnumerable());
            int columnCount = dt.Columns.Count;

            foreach (DataRow row in y) {
                object[] columns = new object[columnCount];

                int i = 0;
                foreach (var item in _seq) {
                    if (item.Item1 == IFunction.function.TableSearchField)
                        columns[i++] = row[item.Item2];
                }

                dt.Rows.Add(columns);
            }

            return dt;
        }
    }
}
