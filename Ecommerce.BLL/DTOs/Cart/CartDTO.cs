using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public record CartItemDto(int ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal Subtotal);
    public record CartDto(int Id, List<CartItemDto> Items, decimal Total);
    public record AddToCartDto(int ProductId, int Quantity);
    public record UpdateCartItemDto(int ProductId, int Quantity);
}
