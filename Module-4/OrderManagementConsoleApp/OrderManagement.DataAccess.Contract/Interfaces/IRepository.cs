using OrderManagement.DataAccess.Contract.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagement.DataAccess.Contract
{
    public interface IRepository<T> 
        where T : BaseModel
    {
        T GetBy(int id);

        IList<T> GetCollection();

        T Insert(T item);

        T Update(T item);

        bool Delete(int id);
    }
}
