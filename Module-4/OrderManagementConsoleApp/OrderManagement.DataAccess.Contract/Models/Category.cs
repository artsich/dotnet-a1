using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.DataAccess.Contract.Models
{
    public class Category : BaseModel
    {
        public string CategoryName { get; set; }

        public string Description { get; set; }
        
        public byte[] Pictire { get; set; }
    }
}
