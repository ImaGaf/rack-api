using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.UseCases.ConfirmReceipt.AppStore;
using RackLogic.Web.Exceptions;
using RackLogic.Web.Models;

namespace RackLogic.Web.ConfirmReceipt.AppStore;

public class ConfirmReceipt(
  IMediator _mediator)
  : Endpoint<ConfirmReceiptRequest, ApiResponse<bool>>
{
  public override void Configure()
  {
    Post("/confirm-receipt/appStoreConfirm");
    AllowAnonymous();
  }

  public override async Task HandleAsync(ConfirmReceiptRequest req, CancellationToken ct)
  {
    var validator = new ConfirmReceiptRequestValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }
    var result = await _mediator.Send(new ConfirmReceiptCommand(req.UserId, req.TransactionId), ct);

    if (result.IsSuccess)
    {
      Response = new ApiResponse<bool>(result.Value);
      return;
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
