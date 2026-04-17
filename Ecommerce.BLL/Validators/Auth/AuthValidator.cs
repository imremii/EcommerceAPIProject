using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name cannot be empty.")
                .WithErrorCode("ERR-01")

                .MaximumLength(50)
                .WithMessage("First name cannot exceed 50 characters.")
                .WithErrorCode("ERR-02");
            
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name cannot be empty.")
                .WithErrorCode("ERR-03")

                .MaximumLength(50)
                .WithMessage("Last name cannot exceed 50 characters.")
                .WithErrorCode("ERR-04");
            
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email cannot be empty.")
                .WithErrorCode("ERR-05")

                .EmailAddress()
                .WithMessage("Invalid email format.")
                .WithErrorCode("ERR-06");
            
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password cannot be empty.")
                .WithErrorCode("ERR-07")

                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .WithErrorCode("ERR-08")

                .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")

                .Matches("[0-9]")
                .WithMessage("Password must contain at least one digit.")
                .WithErrorCode("ERR-09")

                .Matches("[^a-zA-Z0-9]")
                .WithMessage("Password must contain at least one special character.")
                .WithErrorCode("ERR-10");

                 RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match.")
                .WithErrorCode("ERR-11");
        }
    }

    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email cannot be empty.")
                .WithErrorCode("ERR-01")

                .EmailAddress()
                .WithMessage("Invalid email format.")
                .WithErrorCode("ERR-02");


            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password cannot be empty.")
                .WithErrorCode("ERR-03");
        }
    }
}
