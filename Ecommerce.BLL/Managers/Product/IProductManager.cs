using Ecommerce.Common;
using Ecommerce.DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public interface IProductManager
    {
        Task<GeneralResult<PagedResult<ProductDto>>> GetProductsPaginationAsync(PaginationParameters paginationParameters, ProductFilterParameters productFilterParameters);
        Task<GeneralResult<ProductDto>> GetByIdAsync(int id);
        Task<GeneralResult<ProductDto>> CreateAsync(CreateProductDto dto);
        Task<GeneralResult<ProductDto>> UpdateAsync(int id, UpdateProductDto dto);
        Task<GeneralResult<bool>> DeleteAsync(int id);
        Task<GeneralResult<bool>> UpdateImageAsync(int id, string imageUrl);
    }
}
