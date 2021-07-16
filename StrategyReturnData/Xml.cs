using StrategyReturnData.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace StrategyReturnData
{
    public class Xml :IReturnFormatter
    {
        private DataTable _dt;
        public Xml (DataTable dt) {
            _dt = dt;
        }
        public string Convert() {
            using (MemoryStream memoryStream = new MemoryStream()) {
                using (TextWriter streamWriter = new StreamWriter(memoryStream)) {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataSet));
                    xmlSerializer.Serialize(streamWriter, _dt);
                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }
    }
}
