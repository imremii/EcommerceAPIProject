using Ecommerce.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<Product>> GetProductsPagination(PaginationParameters? paginationParameters = null, ProductFilterParameters productFilterParameters = null)
        {
            IQueryable<Product> query = _context.Set<Product>().AsQueryable();

            query = query.Include(p => p.Category);

            if (productFilterParameters != null)
            {
                query = ApplyFilter(query, productFilterParameters);
            }

            var totalCount = await query.CountAsync();

            var pageNumber = paginationParameters?.PageNumber ?? 1;
            var pageSize = paginationParameters?.PageSize ?? totalCount;

            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<Product>
            {
                Items = items,
                Metadata = new PaginationMetadata
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    HasNext = pageNumber < totalPages,
                    HasPrevious = pageNumber > totalPages,
                }
            };
        }


        private IQueryable<Product> ApplyFilter(IQueryable<Product> query, ProductFilterParameters productFilterParameters)
        {
            if (productFilterParameters.MinCount > 0)
            {
                query = query.Where(p => p.Stock > productFilterParameters.MinCount);
            }

            if (productFilterParameters.MaxCount > 0)
            {
                query = query.Where(p => p.Stock < productFilterParameters.MaxCount);
            }

            if (productFilterParameters.MinPrice > 0)
            {
                query = query.Where(p => p.Price > productFilterParameters.MinPrice);
            }

            if (productFilterParameters.MaxPrice > 0)
            {
                query = query.Where(p => p.Price < productFilterParameters.MaxPrice);
            }

            if (!string.IsNullOrEmpty(productFilterParameters.Search))
            {
                query = query.Where(e => e.Name.Contains(productFilterParameters.Search));
            }

            return query;
        }
        public async Task<Product?> GetWithCategoryAsync(int id)
            => await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
    }
}
