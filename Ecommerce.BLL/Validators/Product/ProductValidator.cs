using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.")
                .WithErrorCode("ERR-01")

                .MaximumLength(200)
                .WithMessage("Name cannot exceed 200 characters.")
                .WithErrorCode("ERR-02");


            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Price cannot be empty.")
                .WithErrorCode("ERR-03")

                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.")
                .WithErrorCode("ERR-04");


            RuleFor(x => x.Stock)
                .NotEmpty()
                .WithMessage("Stock cannot be empty.")
                .WithErrorCode("ERR-05")

                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock must be greater than or equal to 0.")
                .WithErrorCode("ERR-06");


            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("Category Id cannot be empty.")
                .WithErrorCode("ERR-07")

                .GreaterThan(0)
                .WithMessage("Category Id must be greater than 0.")
                .WithErrorCode("ERR-08");
        }
    }

    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.")
                .WithErrorCode("ERR-01")

                .MaximumLength(200)
                .WithMessage("Name cannot exceed 200 characters.")
                .WithErrorCode("ERR-02");


            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Price cannot be empty.")
                .WithErrorCode("ERR-03")

                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.")
                .WithErrorCode("ERR-04");


            RuleFor(x => x.Stock)
                .NotEmpty()
                .WithMessage("Stock cannot be empty.")
                .WithErrorCode("ERR-05")

                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock must be greater than or equal to 0.")
                .WithErrorCode("ERR-06");


            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("Category Id cannot be empty.")
                .WithErrorCode("ERR-07")

                .GreaterThan(0)
                .WithMessage("Category Id must be greater than 0.")
                .WithErrorCode("ERR-08");
        }
    }
}
