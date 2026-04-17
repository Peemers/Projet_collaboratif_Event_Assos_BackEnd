using System.Text;
using System.Threading.RateLimiting;
using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Core.Interfaces.Services;
using EventAssos.Core.Interfaces.Tools;
using EventAssos.Core.Services;
using EventAssos.Infrastructure.AzureTools;
using EventAssos.Infrastructure.DataBase.Context;
using EventAssos.Infrastructure.Repositories;
using EventAssos.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;

#region LoggerInitiation

var configuration = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json")
  .AddEnvironmentVariables()
  .Build();

Log.Logger = new LoggerConfiguration() //création logger avant tout le reste dans program
  .MinimumLevel.Override("Microsoft", LogEventLevel.Information) //niveau d'info
  .Enrich.FromLogContext() //
  .WriteTo.Console()
  .WriteTo.AzureBlobStorage(
    connectionString: configuration["ConnectionString:AzureBlobStorageString"],
    storageContainerName:"Api Logs",
    storageFileName:"log.txt")
  .CreateBootstrapLogger(); //log de démarrage

#endregion

try
{
  Log.Information("API en cours de démarrage");
  WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

  #region Injection de dependances

  /*##INJECTION DE DEPENDANCE##*/

  #region DbContext

  //utilisation de dbcontext
  builder.Services.AddDbContext<EventAssosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

  #endregion

  #region CORS

  //utilisation des cors
  var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

  builder.Services.AddCors(option =>
  {
    option.AddPolicy("AllowAngular", policy =>
    {
      policy.WithOrigins(allowedOrigins ?? Array.Empty<string>())
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
  });

  #endregion

  #region ServiceEtc

  //AddScope -> Une instance par requete
  builder.Services.AddScoped<IInscriptionRepository, InscriptionRepository>();
  builder.Services.AddScoped<IInscriptionService, InscriptionService>();
  builder.Services.AddScoped<ICategorieRepository, CategorieRepository>();
  builder.Services.AddScoped<ICategorieService, CategorieService>();
  builder.Services.AddScoped<IEvenementRepository, EvenementRepository>();
  builder.Services.AddScoped<IEvenementService, EvenementService>();
  builder.Services.AddScoped<IEmailService, EmailService>();
  builder.Services.AddScoped<IBlobService, BlobStorageService>();
  
  builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
  builder.Services.AddScoped<IMembreRepository, MembreRepository>();
  builder.Services.AddScoped<IJwtService, JwtService>();
  builder.Services.AddScoped<IAuthService, AuthService>();
  builder.Services.AddControllers();
  builder.Services.AddOpenApi();

  #endregion

  #region JWT

  builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
      };
    });

  #endregion

  #region RateLimit

  builder.Services.AddRateLimiter(options =>
  {
    options.AddFixedWindowLimiter("auth-limit", opt =>
    {
      opt.PermitLimit = 35;
      opt.Window = TimeSpan.FromMinutes(1);
      opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
      opt.QueueLimit = 0;
    });
    options.AddFixedWindowLimiter("RateLimitAdmin", opt =>
    {
      opt.PermitLimit = 40;
      opt.Window = TimeSpan.FromSeconds(10);
      opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
      opt.QueueLimit = 0;
    });
    options.AddFixedWindowLimiter("NormalRequest", opt =>
    {
      opt.PermitLimit = 35;
      opt.Window = TimeSpan.FromSeconds(15);
      opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
      opt.QueueLimit = 0;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
  });

  #endregion

  #endregion

  #region Logging .net -> Serilog

  builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

  #endregion

  var app = builder.Build();


  #region MiddleWare
  
  app.UseHttpsRedirection();
  
  app.UseSerilogRequestLogging(options =>
  {
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
  });

  if (app.Environment.IsDevelopment())
  {
    app.MapScalarApiReference();
    app.MapOpenApi();
  }

  app.UseCors("AllowAngular");
  app.UseRateLimiter();
  
  app.UseAuthentication();
  app.UseAuthorization();
  app.MapControllers();

  #endregion

  Log.Information("API démarrée et prête");
  app.Run();
}
catch (Exception ex)
{
  Log.Fatal(ex, "Erreur de démarrage de l'API");
}
finally
{
  Log.CloseAndFlush();
}