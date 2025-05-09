using System;
using System.Collections.Generic;
namespace ECommerceAPI.Models
{
    public class Order:BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public Guid ShippingAddressId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
