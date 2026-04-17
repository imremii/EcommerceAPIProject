using Ecommerce.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetWithCategoryAsync(int id);
        Task<PagedResult<Product>> GetProductsPagination(PaginationParameters? paginationParameters = null, ProductFilterParameters productFilterParameters = null);
    }
}
