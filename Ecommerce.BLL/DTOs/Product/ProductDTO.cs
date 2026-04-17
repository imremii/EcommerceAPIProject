using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public record ProductDto(int Id, string Name, string? Description, decimal Price, int Stock, string? ImageUrl, int CategoryId, string CategoryName);
    public record CreateProductDto(string Name, string? Description, decimal Price, int Stock, int CategoryId, IFormFile? File = null);
    public record UpdateProductDto(string Name, string? Description, decimal Price, int Stock, int CategoryId, IFormFile? File = null);
}
