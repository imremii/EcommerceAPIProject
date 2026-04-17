using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public class ImageValidator : AbstractValidator<ImageUploadDTO>
    {
        private static readonly string[] AllowedExtentions = { ".png", ".jpg", ".jpeg" };
        public ImageValidator()
        {
            RuleFor(i => i.File)
                .NotNull()
                .WithMessage("File is required")
                .WithErrorCode("ERR-01")
                .WithName("File");

            When(i => i.File != null, () =>
            {
                RuleFor(i => i.File.Length)
                    .GreaterThan(0)
                    .WithMessage("File must be not empty")
                    .WithErrorCode("ERR-02")
                    .WithName("FileSize");

                RuleFor(i => Path.GetExtension(i.File.FileName).ToLower())
                    .Must(ext => AllowedExtentions.Contains(ext))
                    .WithMessage("Unsupported file extention")
                    .WithErrorCode("ERR-03")
                    .WithName("FileExtention");
            });
        }
    }
}
