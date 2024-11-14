using RackLogic.Core.Interfaces;
using RackLogic.Infrastructure;
using RackLogic.Infrastructure.Data.Repositories;
using RackLogic.Infrastructure.Email;

namespace RackLogic.Web.Configurations;

public static class ServiceConfigs
{
  public static IServiceCollection AddServiceConfigs(this IServiceCollection services, ILogger logger, WebApplicationBuilder builder)
  {
    services.AddInfrastructureServices(builder.Configuration, logger)
            .AddMediatrConfigs();


    if (builder.Environment.IsDevelopment())
    {
      // Use a local test email server
      // See: https://ardalis.com/configuring-a-local-test-email-server/
      services.AddScoped<IEmailSender, EmailService>();
      services.AddScoped(typeof(IRepositoryWithExpression<>), typeof(EfRepositoryWithExpression<>));

      // Otherwise use this:
      //builder.Services.AddScoped<IEmailSender, FakeEmailSender>();

    }
    else
    {
      services.AddScoped<IEmailSender, EmailService>();
    }

    logger.LogInformation("{Project} services registered", "Mediatr and Email Sender");

    return services;
  }


}
