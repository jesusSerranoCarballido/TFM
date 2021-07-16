using DataAccess.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using DataAccess.Extensions;
using ODataWebExperimental.Northwind.Model;
using System.Linq;

namespace DataAccess {
    public class Repository { 


        public DataTable Get() {
            var serviceRoot = "https://services.odata.org/V4/Northwind/Northwind.svc/";
            NorthwindEntities context = new NorthwindEntities(new Uri(serviceRoot));

            var ienum = context.Invoices.Execute();
            Extensions.Convert converter = new Extensions.Convert();
            return converter.ToDataTable<NorthwindModel.Invoice>(ienum.ToList());            
        }

        

    }
}
