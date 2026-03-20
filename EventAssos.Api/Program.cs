using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

#region Injection de dependances
/*##INJECTION DE DEPENDANCE##*/

//utilisation de dbcontext
builder.Services.AddDbContext<EventAssosDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

//utilisation des cors
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

builder.Services.AddCors(option =>
{
  option.AddPolicy("AllowAngular", policy =>
  {
    policy.WithOrigins(allowedOrigins ?? Array.Empty<string>())
      .AllowAnyMethod()
      .AllowAnyHeader();
  });
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.MapScalarApiReference();
  app.MapOpenApi();
}

app.UseCors("AllowAngular");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

