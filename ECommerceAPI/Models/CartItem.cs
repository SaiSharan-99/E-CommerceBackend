namespace ECommerceAPI.Models
{
    public class CartItem:BaseEntity
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

    }
}
