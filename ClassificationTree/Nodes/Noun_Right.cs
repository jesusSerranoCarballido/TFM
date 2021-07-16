
using ClassificationTree.Infraestructure.Base;
using ClassificationTree.Nodes.Base;
using ClassificationTree.Transition;
using System;
using System.Collections.Generic;
using System.Linq;
using Wrapper.Spacy;
using static ClassificationTree.Infraestructure.Base.IFunction;

namespace ClassificationTree.Nodes
{
    class Noun_Right :ANodes
    {
        public Noun_Right() : base() { }

        public override IList<(function, string)> Action(Token token, in IList<Token> sentence) {


            if (token.Dep.ToLowerInvariant() == "appos" && base.AntecedentPos.ToLowerInvariant() == "noun") {
                Result.Add((function.TableSearchField, token.Lemma));
            }
            if (token.Dep.ToLowerInvariant() == "compound" && base.AntecedentPos.ToLowerInvariant() == "noun") {
                Result.Add((function.Filter, token.Lemma));
            }
            if (token.Dep.ToLowerInvariant() == "conj" && base.AntecedentPos.ToLowerInvariant() != "noun") {
                Result.Add((function.TableField, token.Lemma));
            }
            if (token.Dep.ToLowerInvariant() == "obl" && base.AntecedentPos.ToLowerInvariant() == "verb") {
                Result.Add((function.TableField, token.Lemma));
            }
            if (token.Dep.ToLowerInvariant() == "obj" && base.AntecedentPos.ToLowerInvariant() == "verb") {
                Result.Add((function.TableField, token.Lemma));
            }
            if (token.Dep.ToLowerInvariant() == "obl" && base.AntecedentPos.ToLowerInvariant() == "adj") {
                Result.Add((function.TableOrderField, token.Lemma));
            }
            if (token.Dep.ToLowerInvariant() == "nmod" && base.AntecedentPos.ToLowerInvariant() == "adv") {
                Result.Add((function.TableOrderField, token.Lemma));
            }
            if (token.Dep.ToLowerInvariant() == "nsubj" && base.AntecedentPos.ToLowerInvariant() == "verb") {
                if (base.AntecedentLemma.ToLowerInvariant() == "decir") {
                    Result.Add((function.TableSearchField, token.Lemma));
                } else {
                    Result.Add((function.TableField, token.Lemma));
                }
            }

            foreach (var child in token.Children) {
                var position = RelativePosition(token, child, sentence);
                Transitions main = new Transitions();

                var state = main.NextINode(child.Dep, child.PoS, position);
                if (state != null) {
                    state.AntecedentPos = token.PoS;
                    state.AntecedentLemma = token.Lemma;
                    var action = state.Action(child, sentence);
                    if (child.PoS.ToLowerInvariant() == "adj") {
                        for (int i = 0; i < action.Count; i++) {
                            if (action[i].Item1 == function.None) {
                                if (Result.Count > 0) {
                                    action[i] = (Result[0].Item1, action[i].Item2);
                                    break;
                                } else {
                                    throw new Exception("creo que hay un error");
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

                } else {
                    if (token.Dep.ToLowerInvariant() == "nmod" && base.AntecedentPos.ToLowerInvariant() == "noun" && token.IsSymbolMoney == false) {
                        if (child.PoS.ToLowerInvariant() == "adp" && child.Dep.ToLowerInvariant() == "case" && position == IMoviments.moviment.Left && child.Lemma.ToLowerInvariant() == "de") {
                            Result.Add((function.TableField, token.Lemma));
                        } else if (child.PoS.ToLowerInvariant() == "adp" && child.Dep.ToLowerInvariant() == "case" && position == IMoviments.moviment.Left && child.Lemma.ToLowerInvariant() == "del") {
                            if (token.IsStop == false) {
                                Result.Add((function.TableSearchField, token.Lemma));
                            } else {
                                Result.Add((function.TableField, token.Lemma));
                            }
                        } else if (child.PoS.ToLowerInvariant() == "adp" && child.Dep.ToLowerInvariant() == "case" && position == IMoviments.moviment.Left && child.Lemma.ToLowerInvariant() == "en") {
                            Result.Add((function.TableField, token.Lemma));
                        } else if (child.PoS.ToLowerInvariant() == "pron" && child.Dep.ToLowerInvariant() == "nmod" && position == IMoviments.moviment.Left && child.Lemma.ToLowerInvariant() == "cuyo") {
                            Result.Add((function.TableField, token.Lemma));
                        } else if (child.PoS.ToLowerInvariant() == "adp" && child.Dep.ToLowerInvariant() == "case" && position == IMoviments.moviment.Left && child.Lemma.ToLowerInvariant() == "con") {
                            Result.Add((function.TableField, token.Lemma));
                        } else if (child.PoS.ToLowerInvariant() == "adp" && child.Dep.ToLowerInvariant() == "case" && position == IMoviments.moviment.Left && child.Lemma.ToLowerInvariant() == "a") {
                            Result.Add((function.TableField, token.Lemma));
                        }
                    } else if (token.Dep.ToLowerInvariant() == "nmod" && base.AntecedentPos.ToLowerInvariant() == "adj" && token.IsSymbolMoney == false) {
                        if (child.Lemma.ToLowerInvariant() == "a") {
                            Result.Add((function.TableField, token.Lemma));
                        } else if (child.Lemma.ToLowerInvariant() == "de") {
                            Result.Add((function.TableField, token.Lemma));
                        }
                    } else if (token.Dep.ToLowerInvariant() == "conj" && base.AntecedentPos.ToLowerInvariant() == "noun" && token.IsSymbolMoney == false) {
                        if (Result.Count < 1) {
                            if (child.Lemma.ToLowerInvariant() == "con") {
                                Result.Add((function.TableField, token.Lemma));
                            } else {
                                Result.Add((function.TableSearchField, token.Lemma));
                            }
                        }
                    }
                }
            }
            if (Result.Count < 1 && token.IsSymbolMoney == false) {
                Result.Add((function.TableSearchField, token.Lemma));
            }
            left = left.Reverse().ToList();
            Result = left.Concat(Result).ToList();
            Result = Result.Concat(right).ToList();

            if (Result.Count == 2 && Result[0].Item1 == function.Filter && Result[1].Item1 == function.TableField) {
                Result = Result.Reverse().ToList();
            }
            if (Result.Count == 3 && Result[0].Item1 == function.Operator && Result[1].Item1 == function.Filter && Result[2].Item1 == function.TableField) {
                var desorderResut = Result.ToList();
                Result.Clear();
                Result.Add(desorderResut[2]);
                Result.Add(desorderResut[0]);
                Result.Add(desorderResut[1]);
            }
            return Result;
        }
    }
}
