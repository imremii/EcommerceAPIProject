using Ecommerce.Common;
using Ecommerce.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _uow;

        public OrderManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<GeneralResult<OrderDto>> PlaceOrderAsync(string userId, PlaceOrderDto dto)
        {
            var cart = await _uow.Carts.GetCartWithItemsAsync(userId);
            if (cart is null || !cart.Items.Any())
                return GeneralResult<OrderDto>.FailResult("Your cart is empty. Add items before placing an order.");

            foreach (var item in cart.Items)
            {
                var product = await _uow.Products.GetByIdAsync(item.ProductId);
                if (product is null || product.Stock < item.Quantity)
                    return GeneralResult<OrderDto>.FailResult($"Insufficient stock for product '{product?.Name ?? "Unknown"}'.");
            }

            var order = new Order
            {
                UserId = userId,
                ShippingAddress = dto.ShippingAddress,
                Items = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.Product!.Price
                }).ToList()
            };
            order.TotalAmount = order.Items.Sum(i => i.Quantity * i.UnitPrice);

            foreach (var item in cart.Items)
            {
                var product = await _uow.Products.GetByIdAsync(item.ProductId);
                product!.Stock -= item.Quantity;
                _uow.Products.Update(product);
            }

            await _uow.Orders.AddAsync(order);

            _uow.Carts.Delete(cart);
            await _uow.SaveChangesAsync();

            var created = await _uow.Orders.GetOrderWithItemsAsync(order.Id, userId);
            return GeneralResult<OrderDto>.SuccessResult(MapToDto(created!), "Order placed successfully.");
        }

        public async Task<GeneralResult<List<OrderDto>>> GetUserOrdersAsync(string userId)
        {
            var orders = await _uow.Orders.GetUserOrdersAsync(userId);
            return GeneralResult<List<OrderDto>>.SuccessResult(orders.Select(MapToDto).ToList());
        }

        public async Task<GeneralResult<OrderDto>> GetOrderByIdAsync(string userId, int orderId)
        {
            var order = await _uow.Orders.GetOrderWithItemsAsync(orderId, userId);
            if (order is null)
                return GeneralResult<OrderDto>.FailResult($"Order with ID {orderId} not found.");
            return GeneralResult<OrderDto>.SuccessResult(MapToDto(order));
        }

        private static OrderDto MapToDto(Order o)
        {
            var items = o.Items.Select(i => new OrderItemDto(
                i.ProductId,
                i.Product?.Name ?? string.Empty,
                i.Quantity,
                i.UnitPrice,
                i.Quantity * i.UnitPrice
            )).ToList();
            return new OrderDto(o.Id, o.OrderDate, o.TotalAmount, o.Status.ToString(), o.ShippingAddress, items);
        }
    }
}
