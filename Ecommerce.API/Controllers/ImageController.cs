using Ecommerce.BLL;
using Ecommerce.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class ImageController : ControllerBase
    {
        private readonly IImageManager _imageManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImageController(IImageManager imageManager, IWebHostEnvironment webHostEnvironment)
        {
            _imageManager = imageManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [Route("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<GeneralResult<ImageUploadResultDto>>> UploadAsync([FromForm(Name = "File")] IFormFile file)
        {
            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var _basePath = _webHostEnvironment.ContentRootPath;
            var imageUploadDTO = new ImageUploadDTO(file);

            var result = await _imageManager.UploadImage(imageUploadDTO, _basePath, schema, host);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }
    }
}
