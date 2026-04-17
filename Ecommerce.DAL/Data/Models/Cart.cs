using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public AppUser User { get; set; } = null!;
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }

    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Cart Cart { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
