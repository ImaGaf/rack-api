using Ardalis.HttpClientTestExtensions;
using RackLogic.Infrastructure.Data;
using RackLogic.Web.Contributors;
using Xunit;

namespace RackLogic.FunctionalTests.ApiEndpoints;

[Collection("Sequential")]
public class ContributorGetById(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client = factory.CreateClient();

  [Fact]
  public async Task ReturnsSeedContributorGivenId1Async()
  {
    var result = await _client.GetAndDeserializeAsync<ContributorRecord>(GetContributorByIdRequest.BuildRoute(1));

    Assert.Equal(1, result.Id);
    Assert.Equal(SeedData.Contributor1.Name, result.Name);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenId1000Async()
  {
    string route = GetContributorByIdRequest.BuildRoute(1000);
    _ = await _client.GetAndEnsureNotFoundAsync(route);
  }
}
