using OrderManagement.DataAccess.Contract.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagement.DataAccess.Contract
{
    public interface IRepository<T>
    {
        T GetBy(int id);

        IList<T> GetCollection();

        void Insert(T item);

        void Update(T item);

        bool Delete(int id);
    }
}
