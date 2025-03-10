using Ardalis.SharedKernel;
using RackLogic.Core.ContributorAggregate;
using RackLogic.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace RackLogic.UnitTests.Core.Services;

public class DeleteContributorService_DeleteContributor
{
  private readonly IRepository<Contributor> _repository = Substitute.For<IRepository<Contributor>>();
  private readonly IMediator _mediator = Substitute.For<IMediator>();
  private readonly ILogger<DeleteContributorService> _logger = Substitute.For<ILogger<DeleteContributorService>>();

  private readonly DeleteContributorService _service;

  public DeleteContributorService_DeleteContributor()
  {
    _service = new DeleteContributorService(_repository, _mediator, _logger);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenCantFindContributorAsync()
  {
    var result = await _service.DeleteContributorAsync(0);

    Assert.Equal(Ardalis.Result.ResultStatus.NotFound, result.Status);
  }
}
