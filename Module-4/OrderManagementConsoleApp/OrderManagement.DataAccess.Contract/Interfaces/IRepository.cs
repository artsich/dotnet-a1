using System.Collections.Generic;
using System.Linq;

namespace OrderManagement.DataAccess.Contract
{
    public interface IQuery<T>
    {
        string Sql { get; }

        ISqlProvider SqlProvider { get; }

        void Execute();
    }

    public interface ISqlProvider
    {
    }

    public interface IRepository<T>
    {
        T GetBy(int id);

        IList<T> GetCollection(string[] includesFields = null);

        T Insert(T item);

        T Update(T item);

        bool Delete(int id);

        IQuery<T> Delete();
    }
}
