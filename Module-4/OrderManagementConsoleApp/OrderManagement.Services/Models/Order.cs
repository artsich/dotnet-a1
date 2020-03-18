using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Services.Models
{

    public enum OrderStatus 
    {
        New,
        InWork,
        Completed
    }

    public class Order
    {
        public int OrderId { get; set; }

        public OrderStatus OrderStatus { get; }
    }
}
