using System;
using System.Collections.Generic;

namespace PenMart.Models.Admin
{
    public class AdminOrderViewModel
    {
        public int OrderId { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsFinalized { get; set; }
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
    }

    public class AdminOrderDetailViewModel
    {
        public int OrderId { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsFinalized { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
