using Ecommerce.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public interface ICategoryManager
    {
        Task<GeneralResult<List<CategoryDto>>> GetAllAsync();
        Task<GeneralResult<CategoryDto>> GetByIdAsync(int id);
        Task<GeneralResult<CategoryDto>> CreateAsync(CreateCategoryDto dto);
        Task<GeneralResult<CategoryDto>> UpdateAsync(int id, UpdateCategoryDto dto);
        Task<GeneralResult<bool>> DeleteAsync(int id);
        Task<GeneralResult<bool>> UpdateImageAsync(int id, string imageUrl);
    }
}
