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
using RackLogic.UseCases.BayConfigurator.List;
using RackLogic.UseCases.FloorPlanItems;
using RackLogic.UseCases.Users;
using RackLogic.UseCases.DefectLogs;

namespace RackLogic.Infrastructure;
public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    services.AddSingleton<AppStoreConfigService>();
    services.AddSingleton<GooglePlayConfigService>();
    string? connectionString = config.GetConnectionString("DefaultConnection");
    Guard.Against.Null(connectionString);
    services.AddDbContext<AppDbContext>(options =>
     options.UseSqlServer(connectionString));

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
           .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
           .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
           .AddScoped<IListBayConfiguratorQueryService, ListBayConfiguratorQueryService>()
           .AddScoped<IDeleteContributorService, DeleteContributorService>()
           .AddScoped<IAuthService, AuthService>()
           .AddScoped<IStoreNotificationService, StoreNotificationService>()
           .AddScoped<IUserWarehouseService, UserWarehouseService>()
           .AddScoped<IFileService, FileService>()
           .AddScoped<IFloorPlanService, FloorPlanService>()
           .AddScoped<IUserTokenRepository, UserTokenRepository>()
           .AddScoped<IUserInspectorService, UserInspectorService>()
           .AddScoped<IFloorPlanItemService, FloorPlanItemService>()
           .AddScoped<IDefectLogHistoryService, DefectLogHistoryService>()
           .AddScoped<IUserService, UserService>()
           .AddScoped<IDefectLogService, DefectLogService>();

    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
