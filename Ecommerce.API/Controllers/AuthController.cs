using Ecommerce.BLL;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public AuthController( IAuthManager authManager,IValidator<RegisterDto> registerValidator,IValidator<LoginDto> loginValidator)
        {
            _authManager = authManager;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var validation = await _registerValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var result = await _authManager.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var validation = await _loginValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            var result = await _authManager.LoginAsync(dto);
            return Ok(result);
        }
    }
}
