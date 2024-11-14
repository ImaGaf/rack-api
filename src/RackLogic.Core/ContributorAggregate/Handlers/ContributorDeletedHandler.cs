using RackLogic.Core.ContributorAggregate.Events;
using RackLogic.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace RackLogic.Core.ContributorAggregate.Handlers;

/// <summary>
/// NOTE: Internal because ContributorDeleted is also marked as internal.
/// </summary>
internal class ContributorDeletedHandler(ILogger<ContributorDeletedHandler> logger) : INotificationHandler<ContributorDeletedEvent>
{
  public Task Handle(ContributorDeletedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Handling Contributed Deleted event for {contributorId}", domainEvent.ContributorId);

    //await emailSender.SendEmailAsync("to@test.com",
    //                                 "from@test.com",
    //                                 "Contributor Deleted",
    //                                 $"Contributor with id {domainEvent.ContributorId} was deleted.");

    return Task.CompletedTask;
  }
}
