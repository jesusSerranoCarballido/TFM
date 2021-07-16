using ClassificationTree.Infraestructure.Base;
using ClassificationTree.Nodes.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Wrapper.Spacy;

namespace ClassificationTree.Nodes
{
    class Cconj_Left :ANodes
    {
        public Cconj_Left() : base() { }
        public override IList<(IFunction.function, string)> Action(Token token, in IList<Token> sentence) {
            if(token.Lemma.Equals("y", StringComparison.InvariantCultureIgnoreCase)) {
               Result.Add((IFunction.function.Conjunction, "AND"));
            }
            if (token.Lemma.Equals("o", StringComparison.InvariantCultureIgnoreCase)) {
                Result.Add((IFunction.function.Conjunction, "OR"));
            }

            return Result;
        }
    }
}
