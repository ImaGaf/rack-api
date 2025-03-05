using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RackLogic.Core.UserAggregate;
using RackLogic.Infrastructure.Data;
using RackLogic.Infrastructure.Token;
using RackLogic.Web.Configurations;
using RackLogic.Web.Middlewares;
using RackLogic.Web.Providers;
using RackLogic.Web.Services;
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

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(op =>
{
  op.Password.RequireNonAlphanumeric = false;
  op.Lockout.MaxFailedAccessAttempts = 30;
  op.Tokens.AuthenticatorTokenProvider = "REFRESHTOKENPROVIDER";
  op.Tokens.PasswordResetTokenProvider = "RESETPASSWORDTOKENPROVIDER";
  op.Tokens.EmailConfirmationTokenProvider = "DELETEPASSWORDTOKENPROVIDER";
})
    .AddSignInManager()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddTokenProvider<PasswordResetTokenProvider<ApplicationUser>>("RESETPASSWORDTOKENPROVIDER")
    .AddTokenProvider<RefreshTokenProvider<ApplicationUser>>("REFRESHTOKENPROVIDER")
    .AddTokenProvider<PasswordResetTokenProvider<ApplicationUser>>("DELETEPASSWORDTOKENPROVIDER");

builder.Services.Configure<RefreshTokenDataProtectionTokenProviderOptions>(options =>
{
  options.TokenLifespan = TimeSpan.FromSeconds(appSettings.RefreshTokenExpireSeconds);
});

builder.Services.Configure<PasswordResetTokenDataProtectionTokenProviderOptions>(options =>
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

builder.Services.AddHostedService<ResetCountService>();

builder.Services.AddHostedService<DeleteUserJob>();

var app = builder.Build();
//if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
//{
//  app.UseSwagger();
//  app.UseSwaggerUi();
//}
await app.UseAppMiddleware();
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".heic"] = "image/heic";
provider.Mappings[".heif"] = "image/heif";

app.UseStaticFiles(new StaticFileOptions
{
  ContentTypeProvider = provider
});
app.UseHttpsRedirection();
app.UseCors("webAppRequests");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseMiddleware<GlobalExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  db.Database.Migrate();
  var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
  await AppDbContextSeed.SeedRolesAsync(roleManager);
}

app.Run();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program { }
