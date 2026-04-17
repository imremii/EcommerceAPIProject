using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public record CategoryDto(int Id, string Name, string? Description, string? ImageUrl, int ProductCount);
    public record CreateCategoryDto(string Name, string? Description, IFormFile? File = null);
    public record UpdateCategoryDto(string Name, string? Description, IFormFile? File = null);
}
