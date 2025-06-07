using CookBook.Data;
using CookBook.Interfaces;
using CookBook.Models;
using CookBook.Services;
using CookBook.Validators.Account;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddControllers();
builder.Services.AddScoped<IAccountService, AccountService>();


builder.Services.AddValidatorsFromAssemblyContaining<RegisterAccountRequestDtoValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{ }

app.UseAuthentication();
app.UseAuthorization();

// app.UseHttpsRedirection();
app.MapControllers();

app.Run();