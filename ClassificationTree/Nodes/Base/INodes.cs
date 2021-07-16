using System;
using System.Collections.Generic;
using System.Text;
using Wrapper.Spacy;
using static ClassificationTree.Infraestructure.Base.IFunction;

namespace ClassificationTree.Nodes.Base
{
    public interface INodes
    {
        string AntecedentPos { get; set; }
        string AntecedentLemma { get; set; }


        public IList<(function, string)> Action(Token token,in IList<Token> sentence);
    }
}
