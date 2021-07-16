using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Base {
    public interface IRepository<T> where T: class {
        Task<IEnumerable<T>> GetAsync();
    }
}
