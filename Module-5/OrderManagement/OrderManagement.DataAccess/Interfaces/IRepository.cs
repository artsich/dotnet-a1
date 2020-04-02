using System.Collections.Generic;

namespace OrderManagement.DataAccess.Interfaces
{
    public interface IRepository<T>
    {
        T Get(int id);

        IList<T> GetAll();

        T Insert(T item);

        void InsertMany(ICollection<T> entities);

        int TryInsertMany(ICollection<T> entities);

        bool Update(T item);

        bool Delete(T item);
    }
}
