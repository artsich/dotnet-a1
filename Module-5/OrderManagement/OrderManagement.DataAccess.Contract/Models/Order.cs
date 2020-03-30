using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.DataAccess.Contract.Models
{
    [Flags]
    public enum OrderStatus
    {
        New = 0,
        InProgress = 1,
        IsDone = 2
    }

    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        
        public OrderStatus Status
        {
            get
            {
                if (OrderDate == null) 
                    return OrderStatus.New;
                if (ShippedDate == null) 
                    return OrderStatus.InProgress;
                else 
                    return OrderStatus.IsDone;
            }
        }

        public string CustomerId { get; set; }

        public int? EmployeeId { get; set; }

        public DateTime? OrderDate { get; }

        public DateTime? RequiredDate { get; set; }

        public DateTime? ShippedDate { get; }

        public int? ShipVia { get; set; }

        public decimal? Freight { get; set; }

        public string ShipName { get; set; }

        public string ShipAddress { get; set; }

        public string ShipCity { get; set; }

        public string ShipRegion { get; set; }

        public string ShipPostalCode { get; set; }

        public string ShipCountry { get; set; }

        public IList<OrderDetail> OrderDetails { get; set; }

        public Order()
        {
        }
    }
}
