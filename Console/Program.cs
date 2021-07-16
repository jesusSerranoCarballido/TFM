
using DataAccess;
using NlpDT;
using StrategyReturnData;
using StrategyReturnData.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NLpDT
{
    class Program
    {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");

            NLPDT nlp = new NLPDT();
            DataTable invoice = new Repository().Get();

            IDictionary<string, string> synonyms = new Dictionary<string, string>();
            synonyms.Add("Precio extendido", "ExtendedPrice");
            synonyms.Add("ciudad", "city");
            synonyms.Add("clienteid", "CustomerID");
            synonyms.Add("nombre cliente", "ProductName");
            synonyms.Add("código", "CustomerID");
            synonyms.Add("cliente", "CustomerID");
            synonyms.Add("nombre", "ProductName");
            synonyms.Add("producto", "ShipName");
            synonyms.Add("cantidad", "Quantity");
            synonyms.Add("pais", "Country");
            synonyms.Add("region", "Region");

            IReturnFormatter convert = new Json(nlp.Run(invoice, "¿Cuál es el precio extendido y el nombre cuya ciudad es Berlin y el cliente es Alfki ordenado descendentemente" +
                                            " por precio extendido y ordenado ascendentemente por nombre?", synonyms));
            string result = convert.Convert();
        } 
    }
}
