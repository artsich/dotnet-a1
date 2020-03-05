using CoreProject.DataAccess.Repository;
using CoreProject.Models;
using Di;

namespace DiTest.TestProject.Services
{
    public class ShopService
    {
        [Inject]
        public IRepository<User> UserRepository { get; set; }

        [Inject]
        public IRepository<Product> ProductRepository { get; set; }
    }
}
