
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RackLogic.Core.UserAggregate;

namespace RackLogic.Web.Services;

public class ResetCountService : BackgroundService
{
  private readonly IServiceProvider _serviceProvider;
  private readonly TimeSpan _resetInterval = TimeSpan.FromDays(1);

  public ResetCountService(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      await ResetFailedLoginCountsAsync();
      await Task.Delay(_resetInterval, stoppingToken);
    }
  }

  private async Task ResetFailedLoginCountsAsync()
  {

    using (var scope = _serviceProvider.CreateScope())
    {
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
      var users = await userManager.Users.ToListAsync();

      foreach (var user in users)
      {
        user.AccessFailedCount = 0;
        user.OtpSendCount = 0;
        await userManager.UpdateAsync(user);
      }
    }
  }
}
