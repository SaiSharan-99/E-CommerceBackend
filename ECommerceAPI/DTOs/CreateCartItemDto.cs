using System;
namespace ECommerceAPI.DTOs
{
    public class CreateCartItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
