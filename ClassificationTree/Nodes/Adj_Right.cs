using ClassificationTree.Infraestructure.Base;
using ClassificationTree.Nodes.Base;
using ClassificationTree.Transition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper.Spacy;
using static ClassificationTree.Infraestructure.Base.IFunction;

namespace ClassificationTree.Nodes
{
    class Adj_Right :ANodes
    {
        public Adj_Right() : base() { }
        public override IList<(IFunction.function, string)> Action(Token token, in IList<Token> sentence) {
            if (token.Dep.ToLowerInvariant() == "amod" && base.AntecedentPos.ToLowerInvariant() == "noun" && token.Lemma.Contains("menor", StringComparison.InvariantCultureIgnoreCase)) {
                Result.Add((function.Operator, "<"));
            } else if (token.Dep.ToLowerInvariant() == "amod" && base.AntecedentPos.ToLowerInvariant() == "noun" && token.Lemma.Contains("inferior", StringComparison.InvariantCultureIgnoreCase)) {
                Result.Add((function.Operator, "<"));
            } else if (token.Dep.ToLowerInvariant() == "amod" && base.AntecedentPos.ToLowerInvariant() == "noun" && token.Lemma.Contains("mayor", StringComparison.InvariantCultureIgnoreCase)) {
                Result.Add((function.Operator, ">"));
            } else if (token.Dep.ToLowerInvariant() == "amod" && base.AntecedentPos.ToLowerInvariant() == "noun" && token.Lemma.Contains("superior", StringComparison.InvariantCultureIgnoreCase)) {
                Result.Add((function.Operator, ">"));
            } else if (token.Dep.ToLowerInvariant() == "advmod" && token.Lemma.Contains("igual", StringComparison.InvariantCultureIgnoreCase)) {
                Result.Add((function.Operator, "="));
            } else if (token.Dep.ToLowerInvariant() == "conj" && token.Lemma.Contains("igual", StringComparison.InvariantCultureIgnoreCase)) {
                Result.Add((function.Operator, "="));
            } else if (token.Dep.ToLowerInvariant() == "amod" && token.Lemma.Contains("igual", StringComparison.InvariantCultureIgnoreCase)) {
                Result.Add((function.Operator, "="));
            } else {
                Result.Add((function.None, token.Lemma));
            }
            foreach (var child in token.Children) {
                var position = RelativePosition(token, child, sentence);
                Transitions main = new Transitions();
                var state = main.NextINode(child.Dep, child.PoS, position);
                if (state != null) {
                    state.AntecedentPos = token.PoS;
                    state.AntecedentLemma = token.Lemma;
                    var action = state.Action(child, sentence);
                    if (token.Lemma.Contains("ordena", StringComparison.InvariantCultureIgnoreCase)) {
                        if (Result.Count > 0) {
                            Result.Clear();
                            Result.Add((function.Order, "ASC"));
                        }
                        if (child.PoS.ToLowerInvariant() == "adv" && child.Dep.ToLowerInvariant() == "advmod") {
                            for (int i = 0; i < action.Count; i++) {
                                if (action[i].Item1 == function.None) {
                                    action[i] = (function.Order, action[i].Item2);
                                    Result.Clear();
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
            //}

            left = left.Reverse().ToList();
            Result = left.Concat(Result).ToList();
            Result = Result.Concat(right).ToList();
            return Result;
        }
    }
}
