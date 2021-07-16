using System;
using System.Collections.Generic;
using System.Text;
using Wrapper.Spacy;
using static ClassificationTree.Infraestructure.Base.IFunction;

namespace ClassificationTree.Infraestructure.Base
{
    public interface ICT
    {    
        public IList<(function, string)>  Plant(IList<Token> tokens);
    }
}
