using Ecommerce.BLL;
using Ecommerce.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;
        private readonly IImageManager _imageManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(ICategoryManager categoryManager, IImageManager imageManager, IWebHostEnvironment webHostEnvironment)
        {
            _categoryManager = categoryManager;
            _imageManager = imageManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryManager.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryManager.GetByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var result = await _categoryManager.CreateAsync(dto);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(new { message = "The category is created successfully" });
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
        {
            var result = await _categoryManager.UpdateAsync(id, dto);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryManager.DeleteAsync(id);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("/api/categories/{id:int}/image")]
        [Authorize(Policy = "AdminOnly")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<GeneralResult<bool>>> UploadImageAsync(int id, [FromForm(Name = "File")] IFormFile file)
        {
            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var basePath = _webHostEnvironment.ContentRootPath;
            var imageUploadDTO = new ImageUploadDTO(file);

            var uploadResult = await _imageManager.UploadImage(imageUploadDTO, basePath, schema, host);
            if (!uploadResult.IsSuccess || uploadResult.Data is null)
                return BadRequest(uploadResult);

            var result = await _categoryManager.UpdateImageAsync(id, uploadResult.Data.ImageURL);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }
    }
}
