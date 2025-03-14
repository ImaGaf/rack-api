using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.Core.UserAggregate;
using RackLogic.UseCases.StoreNotifications;
using RackLogic.UseCases.StoreNotifications.GooglePlay;

namespace RackLogic.UseCases.ConfirmReceipt.GooglePlay;
public class ConfirmReceiptGooglePlayHandler(
  UserManager<ApplicationUser> _userManager,
  IRepositoryWithExpression<PurchaseRecord> _purchaseRecordRepository,
  IStoreNotificationService _storeNotificationService)
  : ICommandHandler<ConfirmReceiptGooglePlayCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(ConfirmReceiptGooglePlayCommand request, CancellationToken cancellationToken)
  {
    var user = await _userManager.FindByIdAsync(request.UserId.ToString());
    if (user == null) return Result.Invalid(new ValidationError("User not found."));
    // Confirm transaction by GooglePlay API.
    var subscriptionPurchase = await _storeNotificationService.VerifyGooglePlayPurchaseTokenAsync(request.ProductId, request.PurchaseToken);
    if (subscriptionPurchase is null) return Result.Invalid(new ValidationError("One of the three pieces of information: PurchaseToken, PackageName, or ProductId is incorrect."));

    if (await TransactionIdExistsAsync(request.PurchaseToken)) return Result.Invalid(new ValidationError("The transaction already exists."));

    return await CreatePurchaseRecordAsync(subscriptionPurchase, user.Id, request.ProductId, request.PurchaseToken, cancellationToken);
  }

  private async Task<bool> TransactionIdExistsAsync(string transactionId)
  {
    var result = await _purchaseRecordRepository.FirstOrDefaultAsync(x => x.PurchaseToken == transactionId);
    return result is not null;
  }

  private async Task<bool> CreatePurchaseRecordAsync(SubscriptionPurchase transactionInfo, Guid userId, string productId, string purchaseToken, CancellationToken cancellationToken)
  {
    await _purchaseRecordRepository.DeleteAsync(x => x.UserId == userId);

    var entity = new PurchaseRecord()
    {
      UserId = userId,
      PurchaseToken = purchaseToken,
      OrderId = transactionInfo.OrderId,
      ProductId = productId,
      PurchaseTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(transactionInfo.StartTimeMillis)).UtcDateTime,
      ExpiryTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(transactionInfo.ExpiryTimeMillis)).UtcDateTime,
      Status = string.Empty,
      AutoRenewing = transactionInfo.AutoRenewing,
      IsRefunded = transactionInfo.CancelReason == 1,
      ReceiptData = string.Empty,
      CreationTime = DateTime.UtcNow
    };

    var result = await _purchaseRecordRepository.AddAsync(entity, cancellationToken);
    if (result is null) return false;

    return true;
  }
}
