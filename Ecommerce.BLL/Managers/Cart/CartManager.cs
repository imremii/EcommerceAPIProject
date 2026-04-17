using Ecommerce.Common;
using Ecommerce.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public class CartManager : ICartManager
    {
        private readonly IUnitOfWork _uow;

        public CartManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<GeneralResult<CartDto>> GetCartAsync(string userId)
        {
            var cart = await _uow.Carts.GetCartWithItemsAsync(userId);
            if (cart is null)
                return GeneralResult<CartDto>.SuccessResult(new CartDto(0, [], 0), "Cart is empty.");
            return GeneralResult<CartDto>.SuccessResult(MapToDto(cart));
        }

        public async Task<GeneralResult<CartDto>> AddToCartAsync(string userId, AddToCartDto dto)
        {
            var product = await _uow.Products.GetByIdAsync(dto.ProductId);
            if (product is null)
                return GeneralResult<CartDto>.NotFound($"Product with ID {dto.ProductId} not found.");
            if (product.Stock < dto.Quantity)
                return GeneralResult<CartDto>.FailResult($"Insufficient stock. Available: {product.Stock}");

            var cart = await _uow.Carts.GetCartWithItemsAsync(userId);
            if (cart is null)
            {
                cart = new Cart { UserId = userId };
                await _uow.Carts.AddAsync(cart);
                await _uow.SaveChangesAsync();
                cart = await _uow.Carts.GetCartWithItemsAsync(userId);
            }

            var existingItem = cart!.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (existingItem is not null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItem { CartId = cart.Id, ProductId = dto.ProductId, Quantity = dto.Quantity });
            }

            cart.UpdatedAt = DateTime.UtcNow;
            _uow.Carts.Update(cart);
            await _uow.SaveChangesAsync();

            var updatedCart = await _uow.Carts.GetCartWithItemsAsync(userId);
            return GeneralResult<CartDto>.SuccessResult(MapToDto(updatedCart!), "Item added to cart.");
        }

        public async Task<GeneralResult<CartDto>> UpdateCartItemAsync(string userId, UpdateCartItemDto dto)
        {
            var cart = await _uow.Carts.GetCartWithItemsAsync(userId);
            if (cart is null)
                return GeneralResult<CartDto>.NotFound("Cart not found.");
            var item = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (item is null)
                return GeneralResult<CartDto>.NotFound("Product not found in cart.");

            var product = await _uow.Products.GetByIdAsync(dto.ProductId);
            if (product!.Stock < dto.Quantity)
                return GeneralResult<CartDto>.FailResult($"Insufficient stock. Available: {product.Stock}");
            item.Quantity = dto.Quantity;
            cart.UpdatedAt = DateTime.UtcNow;
            _uow.Carts.Update(cart);
            await _uow.SaveChangesAsync();

            var updatedCart = await _uow.Carts.GetCartWithItemsAsync(userId);
            return GeneralResult<CartDto>.SuccessResult(MapToDto(updatedCart!), "Cart updated.");
        }

        public async Task<GeneralResult<bool>> RemoveFromCartAsync(string userId, int productId)
        {
            var cart = await _uow.Carts.GetCartWithItemsAsync(userId);
            if (cart is null)
                return GeneralResult<bool>.NotFound("Cart not found.");
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item is null)
                return GeneralResult<bool>.NotFound("Product not found in cart.");

            cart.Items.Remove(item);
            _uow.Carts.Update(cart);
            await _uow.SaveChangesAsync();
            return GeneralResult<bool>.SuccessResult(true, "Item removed from cart.");
        }

        private static CartDto MapToDto(Cart cart)
        {
            var items = cart.Items.Select(i => new CartItemDto(
                i.ProductId,
                i.Product?.Name ?? string.Empty,
                i.Product?.Price ?? 0,
                i.Quantity,
                (i.Product?.Price ?? 0) * i.Quantity
            )).ToList();
            return new CartDto(cart.Id, items, items.Sum(i => i.Subtotal));
        }
    }
}
