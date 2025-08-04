using RackLogic.Core.Interfaces;
using RackLogic.Infrastructure;
using RackLogic.Infrastructure.AzureBlobFile;
using RackLogic.Infrastructure.Data.Repositories;
using RackLogic.Infrastructure.Email;
using RackLogic.UseCases.Users;
using RackLogic.Web.Services;

namespace RackLogic.Web.Configurations;

public static class ServiceConfigs
{
  public static IServiceCollection AddServiceConfigs(this IServiceCollection services, ILogger logger, WebApplicationBuilder builder)
  {
    services.AddInfrastructureServices(builder.Configuration, logger)
            .AddMediatrConfigs();

    services.AddScoped<IAzureBlobService, AzureBlobService>();
    services.AddScoped<IUser, CurrentUser>();
    services.AddScoped<IEmailSender, EmailService>();
    services.AddScoped(typeof(IRepositoryWithExpression<>), typeof(EfRepositoryWithExpression<>));
    logger.LogInformation("{Project} services registered", "Mediatr and Email Sender");

    return services;
  }
}
