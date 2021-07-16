using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace StrategyReturnData.Base
{
    public interface IReturnFormatter
    {
        string Convert();
    }
}
