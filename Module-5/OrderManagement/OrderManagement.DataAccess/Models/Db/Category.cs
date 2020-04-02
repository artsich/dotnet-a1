using DapperExtensions.Mapper;

namespace OrderManagement.DataAccess.Models.Db
{
    public class Category
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }
    }

    internal class CategoryMapper : ClassMapper<Category>
    {
        public CategoryMapper()
        {
            Table("[dbo].[Categories]");
            AutoMap();
        }
    }
}
