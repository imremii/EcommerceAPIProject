using Ecommerce.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public interface IImageManager
    {
        public Task<GeneralResult<ImageUploadResultDto>> UploadImage(ImageUploadDTO imageUploadDTO, string basePath, string? schema, string? host);
    }   
}
