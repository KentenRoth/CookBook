using CookBook.Models;
using FluentValidation;

namespace CookBook.Validators;

public class AppUserValidator : AbstractValidator<AppUser>
{
    public AppUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.")
            .Matches("^[a-zA-Z0-9 ]*$").WithMessage("Name must not contain special characters.");
    }
}