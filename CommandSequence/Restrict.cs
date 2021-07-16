using CommandSequence.Commands;
using CommandSequence.Commands.Base;
using ClassificationTree.Infraestructure.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static ClassificationTree.Infraestructure.Base.IFunction;

namespace CommandSequence
{
    public class Restrict
    {
        private IInterpreter inter;
        public Restrict(IList<(function, string)> seq, DataTable dt) {
            IList<(function, string)> _selectSeq = GetSelectFromSequence(seq);
            IList<(function, string)> _whereSeq = GetWhereFromSequence(seq);
            IList<(function, string)> _orderBy = GetOrderFromSequence(seq);

            inter = new Interpreter(dt);
            if(_whereSeq.Count > 0) {
                inter = new Where(inter, _whereSeq);
            }
            if(_selectSeq.Count > 0) {
                inter = new Select(inter, _selectSeq);
            } 
            if(_orderBy.Count> 0) {
                inter = new Order(inter, _orderBy);
            }
        }

        private IList<(function, string)> GetSelectFromSequence(IList<(function, string)> seq) {
            IList<(function, string)> _selectSeq = new List<(function, string)>();
            for (int i = 0; i < seq.Count; i++) {
                if (seq[i].Item1 == function.TableSearchField) {
                    for (int j = i; j < seq.Count; j++) {
                        i = j;
                        if (seq[j].Item1 == function.TableField || seq[i].Item1 == function.Filter || seq[j].Item1 == function.Order || seq[j].Item1 == function.TableOrderField) {
                            break;
                        } else {
                            _selectSeq.Add(seq[j]);
                        }
                    }
                }
            }
            return _selectSeq;
        }
        private IList<(function, string)> GetWhereFromSequence(IList<(function, string)> seq) {
            IList<(function, string)> _whereSeq = new List<(function, string)>();
            for (int i = 0; i < seq.Count; i++) {
                if (seq[i].Item1 == function.TableField || seq[i].Item1 == function.Filter || seq[i].Item1 == function.Operator) {
                    for (int j = i; j < seq.Count; j++) {
                        i = j;
                        if (seq[j].Item1 == function.TableSearchField || seq[j].Item1 == function.Order || seq[j].Item1 == function.TableOrderField) {
                            break;
                        } else {
                            _whereSeq.Add(seq[j]);
                        }
                    }
                }
            }
            return _whereSeq;
        }
        private IList<(function, string)> GetOrderFromSequence(IList<(function, string)> seq) {
            IList<(function, string)> _orderBy = new List<(function, string)>();
            for (int i = 0; i < seq.Count; i++) {
                if (seq[i].Item1 == function.Order) {
                    for (int j = i; j < seq.Count; j++) {
                        i = j;
                        if (seq[j].Item1 == function.TableSearchField || seq[j].Item1 == function.TableField || seq[i].Item1 == function.Filter) {
                            break;
                        } else {
                            _orderBy.Add(seq[j]);
                        }
                    }
                }
            }
            return _orderBy;
        }
        public DataTable GetData() {
            return inter.Execute(); 
        }
    }
}
