using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL
{
    public class OrderRepository : GenericRepository<Order> , IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }
        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
         => await _context.Orders
             .Include(o => o.Items)
             .ThenInclude(i => i.Product)
             .Where(o => o.UserId == userId)
             .OrderByDescending(o => o.OrderDate)
             .ToListAsync();

        public async Task<Order?> GetOrderWithItemsAsync(int id, string userId)
            => await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
    }
}
