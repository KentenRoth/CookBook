using CookBook.Data;
using CookBook.DTOs.Account.Request;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Validators.Account;

public class UpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    private readonly ApplicationDBContext _dbContext;

    public UpdateUserRequestDtoValidator(ApplicationDBContext dbContext)
    {
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9 ]*$").WithMessage("Name must not contain special characters.");

        RuleFor(x => x.Username)
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.")
            .MustAsync(async (username, cancellationToken) =>
            {
                return !await dbContext.Users.AnyAsync(u => u.UserName == username);
            }).When(x => !string.IsNullOrWhiteSpace(x.Username));

        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(100)
            .MustAsync(async (email, cancellationToken) =>
            {
                return !await dbContext.Users.AnyAsync(u => u.Email == email);
            }).When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.NewPassword)
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.NewPassword));

        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required.");
    }
}