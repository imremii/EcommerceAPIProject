using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order?> GetOrderWithItemsAsync(int id, string userId);
    }
}
