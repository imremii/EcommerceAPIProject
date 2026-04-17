using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public class CreateOrderValidator : AbstractValidator<PlaceOrderDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.ShippingAddress)
                .NotEmpty()
                .WithMessage("Shipping address cannot be empty.")
                .WithErrorCode("ERR-01")

                .MaximumLength(200)
                .WithMessage("Shipping address cannot exceed 200 characters.")
                .WithErrorCode("ERR-02");
        }
    }
}
