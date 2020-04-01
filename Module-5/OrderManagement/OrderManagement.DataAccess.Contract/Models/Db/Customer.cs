using System.Collections.Generic;

namespace OrderManagement.DataAccess.Contract.Models.Db
{
    public class Customer
    {
        public int CustomerID { get; set; }

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

        public IList<CustomerDemo> CustomerDemos { get; set; }

        public IList<CustomerDemographic> CustomerDemographics { get; set; }
    }
}
