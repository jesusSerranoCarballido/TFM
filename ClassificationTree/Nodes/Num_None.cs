using ClassificationTree.Infraestructure.Base;
using ClassificationTree.Nodes.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Wrapper.Spacy;
using static ClassificationTree.Infraestructure.Base.IFunction;

namespace ClassificationTree.Nodes
{
    class Num_None :ANodes
    {
        public Num_None() : base() { }
        public override IList<(function, string)> Action(Token token, in IList<Token> sentence) {
            Result.Add((function.Filter, token.Lemma));
            return Result;
        }
    }
}
