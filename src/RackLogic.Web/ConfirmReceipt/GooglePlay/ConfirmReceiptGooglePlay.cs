using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RackLogic.UseCases.ConfirmReceipt.GooglePlay;
using RackLogic.Web.Exceptions;

namespace RackLogic.Web.ConfirmReceipt.GooglePlay;

public class ConfirmReceiptGooglePlay(IMediator _mediator)
  : Endpoint<ConfirmReceiptGooglePlayRequest, bool>
{
  public override void Configure()
  {
    Post("/confirm-receipt/googlePlayConfirm");
    AllowAnonymous();
  }

  public override async Task HandleAsync(ConfirmReceiptGooglePlayRequest req, CancellationToken ct)
  {
    var validator = new ConfirmReceiptGooglePlayRequestValidator();
    var validationResult = validator.Validate(req);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
      throw new InvalidException(errors);
    }
    var result = await _mediator.Send(new ConfirmReceiptGooglePlayCommand(req.UserId, req.ProductId, req.PurchaseToken), ct);

    if (result.IsSuccess)
    {
      Response = result.Value;
      return;
    }

    if (result.IsInvalid())
    {
      throw new InvalidException(result?.ValidationErrors?.Select(x => x.ErrorMessage).ToList());
    }

    await SendErrorsAsync(statusCode: 500, ct);
  }
}
