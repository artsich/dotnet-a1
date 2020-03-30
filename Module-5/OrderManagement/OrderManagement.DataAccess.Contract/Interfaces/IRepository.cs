using System.Collections.Generic;

namespace OrderManagement.DataAccess.Contract.Interfaces
{
    public interface IRepository<T>
    {
        T Get(int id);

        IList<T> GetAll();

        T Insert(T item);

        void Update(T item);

        void Delete(T item);
    }
}
