using ClassificationTree.Infraestructure.Base;
using ClassificationTree.Transition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ClassificationTree.Infraestructure.Base.IFunction;
using Wrapper.Spacy;
using ClassificationTree.Nodes.Base;

namespace ClassificationTree.Nodes
{
    class Verb_Right :ANodes
    {
        public Verb_Right() : base() { }
        public override IList<(function, string)> Action(Token token, in IList<Token> sentence) {
            foreach (var child in token.Children) {
                var position = RelativePosition(token, child, sentence);
                Transitions main = new Transitions();
                var state = main.NextINode(child.Dep, child.PoS, position);
                if (state != null) {
                    state.AntecedentPos = token.PoS;
                    state.AntecedentLemma = token.Lemma;
                    var action = state.Action(child, sentence);
                    if (token.Lemma.Contains("ordena", StringComparison.InvariantCultureIgnoreCase)) {
                        if (child.PoS.ToLowerInvariant() == "adv" && child.Dep.ToLowerInvariant() == "advmod") {
                            for (int i = 0; i < action.Count; i++) {
                                if (action[i].Item1 == function.None) {
                                    action[i] = (function.Order, action[i].Item2);
                                    Result.Clear();
                                }
                            }
                        } else {
                            for (int i = 0; i < action.Count; i++) {
                                if (action[i].Item1 == function.TableField || action[i].Item1 == function.TableSearchField) {
                                    action[i] = (function.TableOrderField, action[i].Item2);
                                }
                            }
                        }
                    }
                    switch (position) {
                        case IMoviments.moviment.Left:
                            left = action.Concat(left).ToList();
                            break;
                        case IMoviments.moviment.Right:
                        case IMoviments.moviment.None:
                        default:
                            right = right.Concat(action).ToList();
                            break;
                    }

                }

            }
            left = left.Reverse().ToList();
            Result = left.Concat(Result).ToList();
            Result = Result.Concat(right).ToList();
            if (Result.Count == 3 && Result[0].Item1 == function.Operator && Result[1].Item1 == function.TableField && Result[2].Item1 == function.Filter) {
                var desorderResut = Result.ToList();
                Result.Clear();
                Result.Add(desorderResut[1]);
                Result.Add(desorderResut[0]);
                Result.Add(desorderResut[2]);
            }
            return Result;
        }
    }
}
