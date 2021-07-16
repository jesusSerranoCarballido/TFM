using System;
using System.Collections.Generic;
using System.Text;

namespace ClassificationTree.Infraestructure.Base
{
    public interface  IMoviments
    {
        public enum moviment
        {
            Left,
            Right,
            None //Only DEEP =  ROOT
        }
    }
}
