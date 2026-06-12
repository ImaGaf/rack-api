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
    services.AddScoped(typeof(IRepositoryWithExpression<>), typeof(EfRepositoryWithExpression<>));

    var useSendGrid = builder.Configuration.GetValue<bool>("SendGrid:Enable");
    if (useSendGrid)
    {
      services.AddScoped<IEmailSender, SendGridEmailService>();
      return services;
    }

    var acsConnection = builder.Configuration.GetConnectionString("AcsConnection") ??
      throw new Exception("AcsConnection string is not configured.");

    services.AddSingleton<IEmailSender, ACSEmailService>(sp =>
    {
      var emailClient = new Azure.Communication.Email.EmailClient(acsConnection);
      var emailAddrees = builder.Configuration["Acs:SenderAddress"] ?? string.Empty;
      return new ACSEmailService(emailClient, emailAddrees);
    });

    return services;
  }
}
