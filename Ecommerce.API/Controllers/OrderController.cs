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
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager _orderManager;
        private readonly IValidator<PlaceOrderDto> _placeOrderValidator;

        public OrderController(IOrderManager orderManager, IValidator<PlaceOrderDto> placeOrderValidator)
        {
            _orderManager = orderManager;
            _placeOrderValidator = placeOrderValidator;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDto dto)
        {
            var validation = await _placeOrderValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _orderManager.PlaceOrderAsync(userId, dto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _orderManager.GetUserOrdersAsync(userId);
            return Ok(result);
        }

        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetById(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var result = await _orderManager.GetOrderByIdAsync(userId, orderId);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }
    }
}
