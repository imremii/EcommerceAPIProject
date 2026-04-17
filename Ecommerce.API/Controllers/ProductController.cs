using Ecommerce.BLL;
using Ecommerce.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductManager _productManager;
        private readonly IImageManager _imageManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductManager productManager, IImageManager imageManager, IWebHostEnvironment webHostEnvironment)
        {
            _productManager = productManager;
            _imageManager = imageManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParameters paginationParameters, [FromQuery] ProductFilterParameters productFilterParameters)
        {
            var result = await _productManager.GetProductsPaginationAsync(paginationParameters, productFilterParameters);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productManager.GetByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var result = await _productManager.CreateAsync(dto);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(new { message = "The product is created successfully" });
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            var result = await _productManager.UpdateAsync(id, dto);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productManager.DeleteAsync(id);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("/api/products/{id:int}/image")]
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

            var result = await _productManager.UpdateImageAsync(id, uploadResult.Data.ImageURL);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }
    }
}
