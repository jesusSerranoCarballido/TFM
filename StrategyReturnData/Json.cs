using StrategyReturnData.Base;
using System;
using System.Data;
using System.Text.Json;

namespace StrategyReturnData
{
    public class Json:IReturnFormatter
    {
        private DataTable _dt;
        public Json(DataTable dt) {
            _dt = dt;
        }

        public string Convert() {
            return JsonSerializer.Serialize(_dt);
        }
    }
}
