using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetCartWithItemsAsync(string userId);
    }
}
