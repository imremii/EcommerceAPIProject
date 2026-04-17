using Ecommerce.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public class ImageManager : IImageManager
    {
        private readonly ImageValidator _validator;

        public ImageManager(ImageValidator validator)
        {
            _validator = validator;
        }

        public async Task<GeneralResult<ImageUploadResultDto>> UploadImage(ImageUploadDTO imageUploadDTO, string basePath,
            string? schema, string? host)
        {

            if (string.IsNullOrWhiteSpace(schema) || string.IsNullOrWhiteSpace(host))
            {
                return GeneralResult<ImageUploadResultDto>.FailResult("Missing schema or host");
            }

            var res = await _validator.ValidateAsync(imageUploadDTO);
            if (!res.IsValid)
            {
                return GeneralResult<ImageUploadResultDto>.FailResult("Image is not vaild");
            }

            var file = imageUploadDTO.File;
            var extension = Path.GetExtension(file.FileName).ToLower();
            var cleanName = Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "_").ToLower();
            var newFileName = $"{cleanName}_{Guid.NewGuid()}{extension}";


            var directoryPath = Path.Combine(basePath, "Files");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fullFilePath = Path.Combine(directoryPath, newFileName);
            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"{schema}://{host}/Files/{newFileName}";
            var imageUploadResultDto = new ImageUploadResultDto(url);
            return GeneralResult<ImageUploadResultDto>.SuccessResult(imageUploadResultDto);
        }
    }
}
