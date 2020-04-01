using System.Collections.Generic;

namespace OrderManagement.DataAccess.Contract.Interfaces
{
    public interface IRepository<T>
    {
        T Get(int id);

        IList<T> GetAll();

        T Insert(T item);

        bool Update(T item);

        bool Delete(T item);
    }
}
