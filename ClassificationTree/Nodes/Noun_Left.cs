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

    public class Noun_Left :ANodes
    {

        public Noun_Left() : base() { }
        public override IList<(function, string)> Action(Token token, in IList<Token> sentence) {
            if (this.AntecedentPos.ToLowerInvariant() == "propn") {
                Result.Add((function.TableField, token.Lemma));
            } else if (this.AntecedentPos.ToLowerInvariant() == "verb" && token.Dep.ToLowerInvariant() == "obj" ) {        
                Result.Add((function.TableField, token.Lemma));                                                                        
            } else  {
                Result.Add((function.TableSearchField, token.Lemma));
            }

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
                } else {
                    if (this.AntecedentPos.ToLowerInvariant() == "verb" && token.Dep.ToLowerInvariant() == "obl") {
                        if(child.Dep.ToLowerInvariant()=="det" && child.PoS.ToLowerInvariant() == "det" && child.Lemma.ToLowerInvariant() == "cuanto") {
                            Result.Clear();
                            Result.Add((function.TableField, token.Lemma));
                        }
                        
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
