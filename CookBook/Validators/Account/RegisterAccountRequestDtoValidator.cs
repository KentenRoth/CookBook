using CookBook.Data;
using CookBook.DTOs.Account.Request;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Validators.Account;

public class RegisterAccountRequestDtoValidator : AbstractValidator<RegisterAccountRequestDto>
{
    private readonly ApplicationDBContext _dbContext;

    public RegisterAccountRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name is required and cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9 ]*$").WithMessage("Name must not contain special characters.");
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(50).WithMessage("Username is required and cannot exceed 50 characters.")
            .MustAsync(UniqueUsername).WithMessage("Username must be unique.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.")
            .MustAsync(UniqueEmail).WithMessage("Email must be unique.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.");
    }
    
    private async Task<bool> UniqueUsername(string username, CancellationToken cancellationToken)
    {
        return !await _dbContext.Users.AnyAsync(u => u.UserName == username, cancellationToken);
    }
    private async Task<bool> UniqueEmail(string email, CancellationToken cancellationToken)
    {
        return !await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }
}