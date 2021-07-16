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
    class Propn_Right :ANodes
    {
        public Propn_Right() : base() { }
        public override IList<(IFunction.function, string)> Action(Token token, in IList<Token> sentence) {
            Result.Add((function.Filter, token.Text));

            foreach (var child in token.Children) {
                var position = RelativePosition(token, child, sentence);
                Transitions main = new Transitions();

                var state = main.NextINode(child.Dep, child.PoS, position);
                if (state != null) {
                    state.AntecedentPos = token.PoS;
                    state.AntecedentLemma = token.Lemma;
                    var action = state.Action(child, sentence);
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
            return Result;
        }
    }
}
