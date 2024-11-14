using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RackLogic.Core.UserAggregate;
using RackLogic.Infrastructure.Data;
using RackLogic.Infrastructure.Token;
using RackLogic.Web;
using RackLogic.Web.Configurations;
using RackLogic.Web.Middlewares;
using Serilog;
using Serilog.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);
var appSettings = builder.Configuration.GetSection("TokenSettings").Get<TokenSettings>() ?? default!;
builder.Services.AddSingleton(appSettings);
var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

builder.AddLoggerConfigs();

var appLogger = new SerilogLoggerFactory(logger)
    .CreateLogger<Program>();

builder.Services.AddOptionConfigs(builder.Configuration, appLogger, builder);
builder.Services.AddServiceConfigs(appLogger, builder);

builder.Services.AddFastEndpoints()
                .SwaggerDocument(o =>
                {
                  o.ShortSchemaNames = true;
                });

builder.Services.AddIdentityCore<ApplicationUser>(op =>
{
  op.Password.RequireNonAlphanumeric = false;
})
                .AddSignInManager()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("REFRESHTOKENPROVIDER");

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
  options.TokenLifespan = TimeSpan.FromSeconds(appSettings.RefreshTokenExpireSeconds);
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
  options.TokenLifespan = TimeSpan.FromSeconds(appSettings.PasswordRefreshTokenExpireSeconds);
});

builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
  options.RequireHttpsMetadata = false;
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    RequireExpirationTime = true,
    ValidIssuer = appSettings.Issuer,
    ValidAudience = appSettings.Audience,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.SecretKey)),
    ClockSkew = TimeSpan.FromSeconds(0)
  };
});

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
  options.AddPolicy("webAppRequests", builder =>
  {
    builder.AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins(appSettings.Audience)
    .AllowCredentials();
  });
});

builder.Services.AddSwaggerGen(config =>
{
  config.SwaggerDoc("v1", new OpenApiInfo() { Title = "App Api", Version = "v1" });
  config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    In = ParameterLocation.Header,
    Description = "Please enter token",
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    BearerFormat = "JWT",
    Scheme = "bearer"
  });
  config.AddSecurityRequirement(
      new OpenApiSecurityRequirement{
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
      });
});

var app = builder.Build();
//if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
//{
//  app.UseSwagger();
//  app.UseSwaggerUi();
//}
await app.UseAppMiddleware();
app.UseHttpsRedirection();
app.UseCors("webAppRequests");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.Run();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program { }
