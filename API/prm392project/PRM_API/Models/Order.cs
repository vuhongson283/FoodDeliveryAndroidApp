using System;
using System.Collections.Generic;

namespace PRM_API.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public decimal TotalPrice { get; set; }
        public string? OrderStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool? IsDeleted { get; set; }
        public string? PaymentMethod { get; set; }

        public virtual User Customer { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
