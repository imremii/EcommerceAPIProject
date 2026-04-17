using Ecommerce.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public interface ICartManager
    {
        Task<GeneralResult<CartDto>> GetCartAsync(string userId);
        Task<GeneralResult<CartDto>> AddToCartAsync(string userId, AddToCartDto dto);
        Task<GeneralResult<CartDto>> UpdateCartItemAsync(string userId, UpdateCartItemDto dto);
        Task<GeneralResult<bool>> RemoveFromCartAsync(string userId, int productId);
    }
}
