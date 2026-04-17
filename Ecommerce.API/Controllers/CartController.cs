using Ecommerce.BLL;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CustomerOnly")]
    public class CartController : ControllerBase
    {
        private readonly ICartManager _cartManager;
        private readonly IValidator<AddToCartDto> _addToCartValidator;
        private readonly IValidator<UpdateCartItemDto> _updateCartItemValidator;

        public CartController(
            ICartManager cartManager,
            IValidator<AddToCartDto> addToCartValidator,
            IValidator<UpdateCartItemDto> updateCartItemValidator)
        {
            _cartManager = cartManager;
            _addToCartValidator = addToCartValidator;
            _updateCartItemValidator = updateCartItemValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _cartManager.GetCartAsync(userId);
            return Ok(result);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            var validation = await _addToCartValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _cartManager.AddToCartAsync(userId, dto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("items")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto dto)
        {
            var validation = await _updateCartItemValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _cartManager.UpdateCartItemAsync(userId, dto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("items/{productId:int}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _cartManager.RemoveFromCartAsync(userId, productId);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }
    }
}
