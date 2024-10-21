using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain.Entities.Validations
{
    public class UserValidation : AbstractValidator<User>
    {
        public UserValidation()
        {
            RuleFor(f => f.FullName)
                .NotEmpty().WithMessage("The {PropertyName} field must be provided.")
                .Length(2, 100)
                .WithMessage("The {PropertyName} field must be between {MinLength} and {MaxLength} characters.");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("The password is required.")
               .MinimumLength(8).WithMessage("The password must be at least 8 characters long.")
               .Matches(@"[A-Z]").WithMessage("The password must contain at least one uppercase letter.")
               .Matches(@"[a-z]").WithMessage("The password must contain at least one lowercase letter.")
               .Matches(@"\d").WithMessage("The password must contain at least one digit.")
               .Matches(@"[\W_]").WithMessage("The password must contain at least one special character.");

            RuleFor(x => x.Email.Address)
                .NotEmpty().WithMessage("The email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(f => f.Document.Number.Length)
                .Equal(11)
                .WithMessage("The Document field must have {ComparisonValue} characters, but {PropertyValue} was provided.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("The phone number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("A valid phone number must be provided.");

        }
    }
}
