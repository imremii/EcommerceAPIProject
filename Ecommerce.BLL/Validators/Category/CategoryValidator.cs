using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.")
                .WithMessage("ERR-01")

                .MaximumLength(100)
                .WithMessage("Name cannot exceed 100 characters.")
                .WithMessage("ERR-02");


            RuleFor(x => x.Description)
                .MaximumLength(300)
                .WithMessage("Description cannot exceed 300 characters.")
                .WithMessage("ERR-03");
        }
    }

    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.")
                .WithMessage("ERR-01")

                .MaximumLength(100)
                .WithMessage("Name cannot exceed 100 characters.")
                .WithMessage("ERR-02");


            RuleFor(x => x.Description)
                .MaximumLength(300)
                .WithMessage("Description cannot exceed 300 characters.")
                .WithMessage("ERR-03");
        }
    }
}
