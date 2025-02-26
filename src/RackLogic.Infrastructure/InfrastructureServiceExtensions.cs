using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using RackLogic.Core.Interfaces;
using RackLogic.Core.Services;
using RackLogic.Infrastructure.Data;
using RackLogic.Infrastructure.Data.Queries;
using RackLogic.UseCases.Contributors.List;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RackLogic.UseCases.Auths;
using RackLogic.UseCases.Warehouses;
using RackLogic.UseCases.FloorPlans;
using RackLogic.UseCases.StoreNotifications;
using RackLogic.Infrastructure.Configuration;

namespace RackLogic.Infrastructure;
public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    services.AddSingleton<AppStoreConfigService>();
    string? connectionString = config.GetConnectionString("DefaultConnection");
    Guard.Against.Null(connectionString);
    services.AddDbContext<AppDbContext>(options =>
     options.UseSqlServer(connectionString));

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
           .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
           .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
           .AddScoped<IDeleteContributorService, DeleteContributorService>()
           .AddScoped<IUserService, UserSevice>()
           .AddScoped<IStoreNotificationService, StoreNotificationService>()
           .AddScoped<IUserWarehouseService, UserWarehouseService>()
           .AddScoped<IFileService, FileService>()
           .AddScoped<IUserTokenRepository, UserTokenRepository>();


    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
