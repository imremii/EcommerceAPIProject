using Ecommerce.Common;
using Ecommerce.DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _uow;

        public CategoryManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<GeneralResult<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _uow.Categories.GetAllAsync();
            var dtos = categories.Select(MapToDto).ToList();
            return GeneralResult<List<CategoryDto>>.SuccessResult(dtos);
        }

        public async Task<GeneralResult<CategoryDto>> GetByIdAsync(int id)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category is null)
                return GeneralResult<CategoryDto>.NotFound($"Category with ID {id} not found.");
            return GeneralResult<CategoryDto>.SuccessResult(MapToDto(category));
        }

        public async Task<GeneralResult<CategoryDto>> CreateAsync(CreateCategoryDto dto)
        {
            var category = new Category { Name = dto.Name, Description = dto.Description };
            await _uow.Categories.AddAsync(category);
            await _uow.SaveChangesAsync();
            return GeneralResult<CategoryDto>.SuccessResult(MapToDto(category), "Category created successfully.");
        }

        public async Task<GeneralResult<CategoryDto>> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category is null)
                return GeneralResult<CategoryDto>.NotFound($"Category with ID {id} not found.");
            category.Name = dto.Name;
            category.Description = dto.Description;
            _uow.Categories.Update(category);
            await _uow.SaveChangesAsync();
            return GeneralResult<CategoryDto>.SuccessResult(MapToDto(category), "Category updated successfully.");
        }

        public async Task<GeneralResult<bool>> DeleteAsync(int id)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category is null)
                return GeneralResult<bool>.NotFound($"Category with ID {id} not found.");
            _uow.Categories.Delete(category);
            await _uow.SaveChangesAsync();
            return GeneralResult<bool>.SuccessResult(true, "Category deleted successfully.");
        }

        public async Task<GeneralResult<bool>> UpdateImageAsync(int id, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return GeneralResult<bool>.FailResult("Image url is required.");

            var category = await _uow.Categories.GetByIdAsync(id);
            if (category is null)
                return GeneralResult<bool>.NotFound($"Category with ID {id} not found.");

            category.ImageUrl = imageUrl;
            _uow.Categories.Update(category);
            await _uow.SaveChangesAsync();

            return GeneralResult<bool>.SuccessResult(true, "Category image updated successfully.");
        }

        private static CategoryDto MapToDto(Category c) =>
            new(c.Id, c.Name, c.Description, c.ImageUrl, c.Products?.Count ?? 0);
    }
}
