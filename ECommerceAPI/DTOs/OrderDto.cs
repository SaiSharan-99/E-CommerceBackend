using System;
using System.Collections.Generic;
namespace ECommerceAPI.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemDto> Items { get; set; }
        public ShippingAddressDto ShippingAddress { get; set; }
    }
}
