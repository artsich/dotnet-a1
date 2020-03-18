using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.DataAccess.Contract
{
    public interface IRepository<T>
    {
        T GetBy(int guid);
        
        IList<T> GetCollection();

        void Insert(T item);

        void Update(T item);

        void Delete(int guid);
    }
}
