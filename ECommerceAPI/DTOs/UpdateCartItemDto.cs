using System;
namespace ECommerceAPI.DTOs
{
    public class UpdateCartItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
