using DataAccess;
using NUnit.Framework;
using Synonyms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NlpDT.Test
{
    class Tests
    {
        private NlpDT.NLPDT nlp;
        private DataTable invoices;
        private IDictionary<string, string> synonyms;
        [SetUp]
        public void Setup() {
            nlp = new NlpDT.NLPDT();

            invoices = new Repository().Get();

            synonyms = new Dictionary<string, string>();
            synonyms.Add("Precio extendido", "ExtendedPrice");
            synonyms.Add("ciudad", "city");
            synonyms.Add("clienteid", "CustomerID");
            synonyms.Add("nombre cliente", "ProductName");
            synonyms.Add("código", "CustomerID");
            synonyms.Add("cliente", "CustomerID");
            synonyms.Add("nombre", "ProductName");
            synonyms.Add("producto", "ProductName");
            synonyms.Add("cantidad", "Quantity");
            synonyms.Add("pais", "Country");
            synonyms.Add("region", "Region");
            synonyms.Add("carga", "Freight");
        }

        private void CompareList(IList<object> result, DataTable expected, int columnDataTable = 0) {
            Assert.AreEqual(expected.Rows.Count, result.Count, "Number the elements is not equal");

            for (int i = 0; i < result.Count; i++) {
                Assert.AreEqual(Convert.ToString(expected.Rows[i].ItemArray[columnDataTable]),Convert.ToString(result[i]), $"The value in the position {i} is not same");                
            }
        }

        [Test]
        public void Test0() {
            var result0 = nlp.Run(invoices,"En que ciudad se vende el producto Vegie-spread", synonyms);
            var dt0 = invoices.AsEnumerable().Where(x => x.Field<string>("ProductName") == "Vegie-spread").Select(x => x.Field<string>("city")).ToList<object>();
            CompareList(dt0, result0);

            
        }
        [Test]
        public void Test1() {
            var result = nlp.Run(invoices, "¿Cuál es el precio extendido y el nombre del cliente cuya ciudad es Berlin y el código es Alfki?", synonyms);
            var dt = invoices.AsEnumerable().Where(x => x.Field<string>("city").Equals("Berlin", StringComparison.InvariantCultureIgnoreCase) && x.Field<string>("CustomerID").Equals("Alfki", StringComparison.InvariantCultureIgnoreCase)).Select(x => new { ExtendedPrice = x.Field<decimal>("ExtendedPrice"), ProductName =x.Field<object>("ProductName") }).ToList();
            CompareList(dt.Select(x=> Convert.ToString(x.ExtendedPrice)).ToList<object>(), result);
            CompareList(dt.Select(x => Convert.ToString(x.ProductName)).ToList<object>(), result,1);
        }
        [Test]
        public void Test2() {
            var result2 = nlp.Run(invoices, "¿Cual es el precio extendido cuya ciudad es Berlin y el cliente es Alfki ordenado descendentemente por precio extendido?", synonyms);
            var dt2 = invoices.AsEnumerable().Where(x => x.Field<string>("city").Equals("Berlin", StringComparison.InvariantCultureIgnoreCase) && x.Field<string>("CustomerID").Equals("Alfki", StringComparison.InvariantCultureIgnoreCase)).Select(x =>new { ExtendedPrice = x.Field<decimal>("ExtendedPrice") }).OrderByDescending(x=> x.ExtendedPrice).ToList();
            CompareList(dt2.Select(x => Convert.ToString(x.ExtendedPrice)).ToList<object>(), result2);
        }
        [Test]
        public void Test3() {
            var result = nlp.Run(invoices, "¿Cual es el precio extendido, el nombre cuya ciudad es Berlin y el cliente es Alfki ordenado descendentemente por precio extendido y ordenado ascendentemente por nombre?", synonyms);
            var dt = invoices.AsEnumerable().Where(x => x.Field<string>("city").Equals("Berlin", StringComparison.InvariantCultureIgnoreCase) && x.Field<string>("CustomerID").Equals("Alfki", StringComparison.InvariantCultureIgnoreCase)).Select(x => new { ExtendedPrice = x.Field<decimal>("ExtendedPrice"), ProductName = x.Field<object>("ProductName") }).OrderByDescending(x => x.ExtendedPrice).ThenBy(x=>x.ProductName).ToList();
            CompareList(dt.Select(x => Convert.ToString(x.ExtendedPrice)).ToList<object>(), result);
            CompareList(dt.Select(x => Convert.ToString(x.ProductName)).ToList<object>(), result, 1);
        }
        [Test]
        public void Test4() {
            var result = nlp.Run(invoices, "¿Cual es el precio extendido y el nombre cuya ciudad es Berlin y el cliente es Alfi ordenado descendentemente por precio extendido y ordenado ascendentemente por nombre?", synonyms);
            var dt = invoices.AsEnumerable().Where(x => x.Field<string>("city").Equals("Berlin", StringComparison.InvariantCultureIgnoreCase) && x.Field<string>("CustomerID").Equals("Alfi", StringComparison.InvariantCultureIgnoreCase)).Select(x => new { ExtendedPrice = x.Field<decimal>("ExtendedPrice"), ProductName = x.Field<object>("ProductName") }).OrderByDescending(x => x.ExtendedPrice).ThenBy(x => x.ProductName).ToList();
            CompareList(dt.Select(x => Convert.ToString(x.ExtendedPrice)).ToList<object>(), result);
            CompareList(dt.Select(x => Convert.ToString(x.ProductName)).ToList<object>(), result, 1);
        }
        [Test]
        public void Test5() {
            var result = nlp.Run(invoices, "¿Me puedes decir todas las ciudades?", synonyms);
            var dt = invoices.AsEnumerable().Select(x => x.Field<string>("city") ).ToList();
            CompareList(dt.ToList<object>(), result);
        }
        [Test]
        public void Test6() {
            var result = nlp.Run(invoices, "¿Cual es la ciudad y la region del pais de Mexico?", synonyms);
            var dt = invoices.AsEnumerable().Where(x => x.Field<string>("Country").Equals("Mexico", StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Field<string>("city")).ToList();
            CompareList(dt.ToList<object>(), result);

        }
        [Test]
        public void Test7() {
            var result = nlp.Run(invoices, "¿Cual es el precio extendido, el nombre cuya ciudad es Berlin y el cliente es Alfki ordenado descendentemente por precio extendido y ascendentemente por nombre?", synonyms);
            var dt = invoices.AsEnumerable().Where(x => x.Field<string>("city").Equals("Berlin", StringComparison.InvariantCultureIgnoreCase) && x.Field<string>("CustomerID").Equals("Alfki", StringComparison.InvariantCultureIgnoreCase)).Select(x => new { ExtendedPrice = x.Field<decimal>("ExtendedPrice"), ProductName = x.Field<object>("ProductName") }).OrderByDescending(x => x.ExtendedPrice).ThenBy(x => x.ProductName).ToList();
            CompareList(dt.Select(x => Convert.ToString(x.ExtendedPrice)).ToList<object>(), result);
            CompareList(dt.Select(x => Convert.ToString(x.ProductName)).ToList<object>(), result, 1);
        }
        [Test]
        public void Test8() {
            var result = nlp.Run(invoices, "¿Nombre del cliente, precio extendido que tengan un precio extendido inferior a 16?", synonyms);
            var dt = invoices.AsEnumerable().Where(x =>  x.Field<decimal>("ExtendedPrice") < Convert.ToDecimal("16")).Select(x => new { ProductName = x.Field<object>("ProductName"), ExtendedPrice = x.Field<decimal>("ExtendedPrice") }).ToList();
            CompareList(dt.Select(x => Convert.ToString(x.ProductName)).ToList<object>(), result);
            CompareList(dt.Select(x => Convert.ToString(x.ExtendedPrice)).ToList<object>(), result,1);

        }
        [Test]
        public void Test9() {
            var result = nlp.Run(invoices, "¿Nombre del cliente que tengans un precio extendido superior a 16?", synonyms);
            var dt = invoices.AsEnumerable().Where(x => x.Field<decimal>("ExtendedPrice") > Convert.ToDecimal("16")).Select(x => new { ProductName = x.Field<object>("ProductName")}).ToList();
            CompareList(dt.Select(x => Convert.ToString(x.ProductName)).ToList<object>(), result);
        }
        [Test]
        public void Test10() {
            var result = nlp.Run(invoices, "¿Nombre del cliente que tengans un precio extendido menor o igual que 16?", synonyms);
            var dt = invoices.AsEnumerable().Where(x => x.Field<decimal>("ExtendedPrice") <= Convert.ToDecimal("16")).Select(x => new { ProductName = x.Field<object>("ProductName") }).ToList();
            CompareList(dt.Select(x => Convert.ToString(x.ProductName)).ToList<object>(), result);

        }
        [Test]
        public void Test11() {
            var result = nlp.Run(invoices, "¿Nombre del cliente que tengans un precio extendido mayor o igual que 16?", synonyms);
            var dt = invoices.AsEnumerable().Where(x => x.Field<decimal>("ExtendedPrice") >= Convert.ToDecimal("16")).Select(x => new { ProductName = x.Field<object>("ProductName") }).ToList();
            CompareList(dt.Select(x => Convert.ToString(x.ProductName)).ToList<object>(), result);
        }
        [Test]
        public void Test12() {
            var result = nlp.Run(invoices, "¿Nombre del cliente que tengans un precio extendido mayor que 16?", synonyms);
            var dt = invoices.AsEnumerable().Where(x => x.Field<decimal>("ExtendedPrice") > Convert.ToDecimal("16")).Select(x => new { ProductName = x.Field<object>("ProductName") }).ToList();
            CompareList(dt.Select(x => Convert.ToString(x.ProductName)).ToList<object>(), result);

        }
        [Test]
        public void Test13() {
            var result = nlp.Run(invoices, "¿Nombre del cliente que tenga un precio extendido igual que 878.0?", synonyms);
            var dt = invoices.AsEnumerable().Where(x => x.Field<decimal>("ExtendedPrice") == Convert.ToDecimal("878.0")).Select(x => new { ProductName = x.Field<object>("ProductName") }).ToList();
            CompareList(dt.Select(x => Convert.ToString(x.ProductName)).ToList<object>(), result);

        }
        [Test]
        public void Test14() {
            var result = nlp.Run(invoices, "Nombre del cliente mayores de 18 carga?", synonyms);
            var dt = invoices.AsEnumerable().Where(x => x.Field<decimal>("Freight") > Convert.ToDecimal("18")).Select(x => new { ProductName = x.Field<object>("ProductName") }).ToList();
            CompareList(dt.Select(x => Convert.ToString(x.ProductName)).ToList<object>(), result);

        }
        [Test]
        public void Test15() {
            var result = nlp.Run(invoices, "¿Nombre del cliente iguales a 18 carga?", synonyms);
            var dt = invoices.AsEnumerable().Where(x => x.Field<decimal>("Freight") == Convert.ToDecimal("18")).Select(x => new { ProductName = x.Field<object>("ProductName") }).ToList();
            CompareList(dt.Select(x => Convert.ToString(x.ProductName)).ToList<object>(), result);
        }
        [Test]
        public void Test16() {
            var result = nlp.Run(invoices, "¿Nombre del cliente menores de 18 carga?", synonyms);
            var dt = invoices.AsEnumerable().Where(x => x.Field<decimal>("Freight") < Convert.ToDecimal("18")).Select(x => new { ProductName = x.Field<object>("ProductName") }).ToList();
            CompareList(dt.Select(x => Convert.ToString(x.ProductName)).ToList<object>(), result);
        }
        [Test]
        public void Test17() {
            var result = nlp.Run(invoices, "producto en el pais de Germany y con una cantidad mayor a 7.", synonyms);
            var dt = invoices.AsEnumerable().Where(x => x.Field<string>("Country").Equals("Germany", StringComparison.InvariantCultureIgnoreCase) && x.Field<System.Int16>("Quantity") > Convert.ToInt16("7")).Select(x => new { ProductName = x.Field<object>("ProductName") }).ToList();
            CompareList(dt.Select(x => Convert.ToString(x.ProductName)).ToList<object>(), result);

        }
    }
}
