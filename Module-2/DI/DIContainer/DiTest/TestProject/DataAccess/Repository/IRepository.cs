using CoreProject.Models;
using System;
using System.Collections.Generic;

namespace CoreProject.DataAccess.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(Guid id);
        IList<T> GetCollection();
    }
}
