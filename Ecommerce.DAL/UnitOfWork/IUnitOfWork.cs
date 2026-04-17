using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        ICartRepository Carts { get; }
        IOrderRepository Orders { get; }
        IGenericRepository<Category> Categories { get; }
        Task SaveChangesAsync();
    }
}
