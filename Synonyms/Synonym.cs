using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static ClassificationTree.Infraestructure.Base.IFunction;

namespace Synonyms
{
    public class Synonym
    {

        public IList<(function, string)> Convert(IList<(function, string)> seq, DataTable dt, IDictionary<string, string> synonyms) {
            IList<(function, string)> newSeq = Regrouping(seq);
            IList<string> dtColumns = dt.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToList();
            return SynonymsChange(newSeq, dtColumns, synonyms);   
        }


        private IList<(function, string)> Regrouping(IList<(function, string)> seq) {
            IList<(function, string)> group = new List<(function, string)>();
            foreach (var item in seq) {
                if (group.Count == 0) {
                    group.Add(item);
                } else {
                    var lastElementAdd = group[group.Count - 1];
                    if (lastElementAdd.Item1 == item.Item1) {
                        group.RemoveAt(group.Count - 1);
                        group.Add((lastElementAdd.Item1, $"{lastElementAdd.Item2.Trim()} {item.Item2.Trim()}"));
                    } else {
                        group.Add(item);
                    }
                }
            }
            return group;
        }
        private IList<(function, string)> SynonymsChange(IList<(function, string)> newSeq, IList<string> dtColumns, IDictionary<string, string> synonyms) {
            IDictionary<string, string> newSynonymsIgnoreCapitalLetters = new Dictionary<string, string>(synonyms, StringComparer.OrdinalIgnoreCase);
            IList<(function, string)> group = new List<(function, string)>();
            foreach (var item in newSeq) {
                if (item.Item1 == function.TableField || item.Item1 == function.TableSearchField || item.Item1 == function.TableOrderField) {
                    string columnWritten = item.Item2;
                    if (dtColumns.Any(s => s.Equals(columnWritten, StringComparison.OrdinalIgnoreCase))) {
                        group.Add(item);
                    } else {
                        if (newSynonymsIgnoreCapitalLetters.ContainsKey(columnWritten)) {
                            group.Add((item.Item1, newSynonymsIgnoreCapitalLetters[columnWritten]));
                        } else {
                            throw new Exception("No se ha encontrado la columna correspondiente, escriba la pregunta o hable con el administrador para agregar sinonimos");
                        }
                    }
                } else {
                    group.Add(item);
                }
            }
            return group;
        }
    }
}
