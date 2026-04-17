using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public record RegisterDto(string FirstName, string LastName, string Email, string Password, string ConfirmPassword);
    public record LoginDto(string Email, string Password);
    public record AuthResponseDto(string Token, string Email, string FullName, DateTime Expiry);
}
