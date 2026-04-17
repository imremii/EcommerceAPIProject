using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public class AddToCartValidator : AbstractValidator<AddToCartDto>
    {
        public AddToCartValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Id cannot be empty.")
                .WithErrorCode("ERR-01")

                .GreaterThan(0)
                .WithMessage("Id must be greater than 0.")
                .WithErrorCode("ERR-02");


            RuleFor(x => x.Quantity)
                .NotEmpty()
                .WithMessage("Quantity cannot be empty.")
                .WithErrorCode("ERR-03")


                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.")
                .WithErrorCode("ERR-04");
        }
    }

    public class UpdateCartItemValidator : AbstractValidator<UpdateCartItemDto>
    {
        public UpdateCartItemValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Id cannot be empty.")
                .WithErrorCode("ERR-01")

                .GreaterThan(0)
                .WithMessage("Id must be greater than 0.")
                .WithErrorCode("ERR-02");


            RuleFor(x => x.Quantity)
                .NotEmpty()
                .WithMessage("Quantity cannot be empty.")
                .WithErrorCode("ERR-03")


                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.")
                .WithErrorCode("ERR-04");
        }
    }
}
