using Ecommerce.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public interface IOrderManager
    {
        Task<GeneralResult<OrderDto>> PlaceOrderAsync(string userId, PlaceOrderDto dto);
        Task<GeneralResult<List<OrderDto>>> GetUserOrdersAsync(string userId);
        Task<GeneralResult<OrderDto>> GetOrderByIdAsync(string userId, int orderId);
    }
}
