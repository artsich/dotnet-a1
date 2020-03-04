using CoreProject.DataAccess.Context;
using CoreProject.Models;
using System;
using System.Collections.Generic;

namespace CoreProject.DataAccess.Repository
{
    public class UserRepository : IRepository<User>
    {
        private IContext _context;

        public UserRepository(IContext context)
        {
            _context = context;
        }

        public User GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IList<User> GetCollection()
        {
            throw new NotImplementedException();
        }
    }
}
