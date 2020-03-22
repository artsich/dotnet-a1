using OrderManagement.DataAccess.Contract.Models;
using System.Collections.Generic;

namespace OrderManagement.Services.Interfaces
{
    public interface IService<T>
        where T : BaseModel
    {
        T GetById(int id);

        IList<T> GetCollection(T obj);

        void Create(T obj);

        T Update(T obj);

        bool Delete(int id);
    }
}
