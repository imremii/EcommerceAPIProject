using Ecommerce.Common;
using Ecommerce.DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitofwork;


        public ProductManager(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task<GeneralResult<PagedResult<ProductDto>>> GetProductsPaginationAsync(PaginationParameters paginationParameters, ProductFilterParameters productFilterParameters)
        {
            var pagedResult = await _unitofwork.Products.GetProductsPagination(paginationParameters, productFilterParameters);
            var pagedResultDto = new PagedResult<ProductDto>
            {
                Items = pagedResult.Items.Select(MapToDto).ToList(),
                Metadata = pagedResult.Metadata
            };
            return GeneralResult<PagedResult<ProductDto>>.SuccessResult(pagedResultDto);
        }

        public async Task<GeneralResult<ProductDto>> GetByIdAsync(int id)
        {
            var product = await _unitofwork.Products.GetWithCategoryAsync(id);
            if (product is null)
                return GeneralResult<ProductDto>.NotFound($"Product with ID {id} not found.");
            return GeneralResult<ProductDto>.SuccessResult(MapToDto(product));
        }

        public async Task<GeneralResult<ProductDto>> CreateAsync(CreateProductDto dto)
        {
            var categoryExists = await _unitofwork.Categories.ExistsAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists)
                return GeneralResult<ProductDto>.NotFound($"Category with ID {dto.CategoryId} not found.");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId
            };

            await _unitofwork.Products.AddAsync(product);
            await _unitofwork.SaveChangesAsync();

            var created = await _unitofwork.Products.GetWithCategoryAsync(product.Id);
            return GeneralResult<ProductDto>.SuccessResult("Product created successfully.");
        }

        public async Task<GeneralResult<ProductDto>> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _unitofwork.Products.GetWithCategoryAsync(id);
            if (product is null)
                return GeneralResult<ProductDto>.NotFound($"Product with ID {id} not found.");

            var categoryExists = await _unitofwork.Categories.ExistsAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists)
                return GeneralResult<ProductDto>.NotFound($"Category with ID {dto.CategoryId} not found.");
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.CategoryId = dto.CategoryId;

            _unitofwork.Products.Update(product);
            await _unitofwork.SaveChangesAsync();

            var updated = await _unitofwork.Products.GetWithCategoryAsync(product.Id);
            return GeneralResult<ProductDto>.SuccessResult(MapToDto(updated!), "Product updated successfully.");
        }

        public async Task<GeneralResult<bool>> DeleteAsync(int id)
        {
            var product = await _unitofwork.Products.GetByIdAsync(id);
            if (product is null)
                return GeneralResult<bool>.NotFound($"Product with ID {id} not found.");

            _unitofwork.Products.Delete(product);
            await _unitofwork.SaveChangesAsync();
            return GeneralResult<bool>.SuccessResult(true, "Product deleted successfully.");
        }

        public async Task<GeneralResult<bool>> UpdateImageAsync(int id, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return GeneralResult<bool>.FailResult("Image url is required.");

            var product = await _unitofwork.Products.GetByIdAsync(id);
            if (product is null)
                return GeneralResult<bool>.NotFound($"Product with ID {id} not found.");

            product.ImageUrl = imageUrl;
            _unitofwork.Products.Update(product);
            await _unitofwork.SaveChangesAsync();

            return GeneralResult<bool>.SuccessResult(true, "Product image updated successfully.");
        }

        private static ProductDto MapToDto(Product p) =>
            new(p.Id, p.Name, p.Description, p.Price, p.Stock, p.ImageUrl,
                p.CategoryId, p.Category?.Name ?? string.Empty);
    }
}
