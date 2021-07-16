using ClassificationTree.Infraestructure.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper.Spacy;
using static ClassificationTree.Infraestructure.Base.IFunction;
using static ClassificationTree.Infraestructure.Base.IMoviments;

namespace ClassificationTree.Nodes.Base
{
    public abstract class ANodes :INodes
    {
        protected IList<(function, string)> left = new List<(function, string)>();
        protected IList<(function, string)> right = new List<(function, string)>();

        public ANodes() {
            Result = new List<(function, string)>();
        }
        public function Function { get; set; }
        public IList<(function, string)> Result { get; set; }
        public string AntecedentPos { get; set; }
        public string AntecedentLemma { get; set; }

        public abstract IList<(function, string)> Action(Token token,in IList<Token> sentence);

        protected static moviment RelativePosition(Token root, Token tokenRelative,in  IList<Token> sentence) {
            int rootLoc = sentence.IndexOf(root);
            int position = sentence.IndexOf(tokenRelative);
            if (position < rootLoc)
                return moviment.Left;
            if (position > rootLoc)
                return moviment.Right;
            return moviment.None;
        }
    }
}
