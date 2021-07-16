using System;
using System.Collections.Generic;
using System.Text;

namespace ClassificationTree.Infraestructure.Base
{
    public interface IFunction
    {
        public enum function
        {
            None,
            Operator,
            Conjunction,
            TableField,             
            TableSearchField,
            Filter,
            Punct,
            Order,
            TableOrderField
        }
    }
}
