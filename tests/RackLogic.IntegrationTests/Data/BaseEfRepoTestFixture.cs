using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using RackLogic.Core.ContributorAggregate;
using RackLogic.Infrastructure.Data;
using RackLogic.UseCases.Users;

namespace RackLogic.IntegrationTests.Data;

public abstract class BaseEfRepoTestFixture
{
  protected AppDbContext _dbContext;

  protected BaseEfRepoTestFixture()
  {
    var options = CreateNewContextOptions();
    var _fakeEventDispatcher = Substitute.For<IDomainEventDispatcher>();
    var _user = Substitute.For<IUser>();

    _dbContext = new AppDbContext(options, _user, _fakeEventDispatcher);
  }

  protected static DbContextOptions<AppDbContext> CreateNewContextOptions()
  {
    // Create a fresh service provider, and therefore a fresh
    // InMemory database instance.
    var serviceProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .BuildServiceProvider();

    // Create a new options instance telling the context to use an
    // InMemory database and the new service provider.
    var builder = new DbContextOptionsBuilder<AppDbContext>();
    builder.UseInMemoryDatabase("cleanarchitecture")
           .UseInternalServiceProvider(serviceProvider);

    return builder.Options;
  }

  protected EfRepository<Contributor> GetRepository()
  {
    return new EfRepository<Contributor>(_dbContext);
  }
}
