var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{ }

app.UseAuthentication();
app.UseAuthorization();

// app.UseHttpsRedirection();
app.MapControllers();

app.Run();