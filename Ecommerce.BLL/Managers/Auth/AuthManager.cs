using Ecommerce.Common;
using Ecommerce.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.BLL
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;

        public AuthManager(UserManager<AppUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<GeneralResult<AuthResponseDto>> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser is not null)
                return GeneralResult<AuthResponseDto>.FailResult("Email is already registered.");
            var user = new AppUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return GeneralResult<AuthResponseDto>.FailResult("Registration failed.");

            await _userManager.AddToRoleAsync(user, "Customer");

            var token = await GenerateJwtToken(user);
            return GeneralResult<AuthResponseDto>.SuccessResult(token, "Registration successful.");
        }

        public async Task<GeneralResult<AuthResponseDto>> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return GeneralResult<AuthResponseDto>.FailResult("Invalid email or password.");
            var token = await GenerateJwtToken(user);
            return GeneralResult<AuthResponseDto>.SuccessResult(token, "Login successful.");
        }

        private async Task<AuthResponseDto> GenerateJwtToken(AppUser user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"]!));
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.Email!),
                new("firstName", user.FirstName),
                new("lastName", user.LastName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds);

            return new AuthResponseDto(
                new JwtSecurityTokenHandler().WriteToken(token),
                user.Email!,
                $"{user.FirstName} {user.LastName}",
                expiry);
        }
    }
}
