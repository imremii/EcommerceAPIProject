using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public record OrderItemDto(int ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal Subtotal);
    public record OrderDto(int Id, DateTime OrderDate, decimal TotalAmount, string Status, string? ShippingAddress, List<OrderItemDto> Items);
    public record PlaceOrderDto(string? ShippingAddress);
}
