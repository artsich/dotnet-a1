using CoreProject.DataAccess.Context;
using CoreProject.Models;
using System;
using System.Collections.Generic;

namespace CoreProject.DataAccess.Repository
{
    public class ProductRepository : IRepository<Product>
    {
        private IContext _context;

        public ProductRepository(IContext context)
        {
            _context = context;
        }

        public Product GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IList<Product> GetCollection()
        {
            throw new NotImplementedException();
        }
    }
}
