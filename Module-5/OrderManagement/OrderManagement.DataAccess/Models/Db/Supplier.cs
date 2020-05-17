using DapperExtensions.Mapper;

namespace OrderManagement.DataAccess.Models.Db
{
    public class Supplier
    {
        public int SupplierID { get; set; }

        public string CompanyName { get; set; }

        public string ContactName { get; set; }

        public string ContactTitle { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string HomePage { get; set; }
    }

    internal class SupplierMapper : ClassMapper<Supplier>
    {
        public SupplierMapper()
        {
            Table("[dbo].[Suppliers]");
            AutoMap();
        }
    }
}