using System.Collections.Generic;

namespace OrderManagement.Services.Interfaces
{
    public interface IService<T>
    {
        void Insert(T entity);

        void InsertMany(IList<T> entities);
    }
}
