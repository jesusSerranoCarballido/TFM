using ClassificationTree.Infraestructure.Base;
using ClassificationTree.Nodes.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Wrapper.Spacy;
using static ClassificationTree.Infraestructure.Base.IFunction;

namespace ClassificationTree.Nodes
{
    class Punct_None :ANodes
    {
        public Punct_None() : base() { }
        public override IList<(IFunction.function, string)> Action(Token token, in IList<Token> sentence) {
            if (token.Lemma == "," && token.IsPunct == true) {
                Result.Add((function.Punct, ","));
            }

            return Result;
        }
    }
}
