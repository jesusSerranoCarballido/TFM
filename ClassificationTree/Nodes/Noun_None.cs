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
    class Noun_None :ANodes
    {
        public Noun_None() : base() { }

        public override IList<(IFunction.function, string)> Action(Token token, in IList<Token> sentence) {
            Result.Add((function.TableSearchField, token.Lemma));

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
                                if (Result.Count >= 0) {
                                    action[i] = (Result[0].Item1, action[i].Item2);
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

                }

            }
            Result = left.Concat(Result).ToList();
            Result = Result.Concat(right).ToList();
            return Result;
        }
    }
}
