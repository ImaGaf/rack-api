using System.Text;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Newtonsoft.Json;

namespace RackLogic.UseCases.StoreNotifications.AppStore;
public class AppStoreNotificationsHandler(IStoreNotificationService _storeNotificationService) 
  : ICommandHandler<AppStoreNotificationPayloadCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(AppStoreNotificationPayloadCommand request, CancellationToken cancellationToken)
  {
    var tran = await GetTransactionInfoByToken(request.SignedPayload);
    var rs = _storeNotificationService.VerifyAppleReceiptByTransactionId(tran?.TransactionId ?? string.Empty);
    return await VerifyAppleReceipt(request.SignedPayload);
  }

  private async Task<TransactionInfo?> GetTransactionInfoByToken(string token)
  {
    var appStoreNotification = await _storeNotificationService.DecodeJWTToken<AppStoreNotification>(token);
    if (appStoreNotification is null) return null;

    return await _storeNotificationService.DecodeJWTToken<TransactionInfo>(appStoreNotification.Data?.SignedTransactionInfo ?? string.Empty);
  }

  private async Task<bool> VerifyAppleReceipt(string request)
  {
    var url = "https://sandbox.itunes.apple.com/verifyReceipt";
    var appStoreNotification = await _storeNotificationService.DecodeJWTToken<AppStoreNotification>(request);
    if (appStoreNotification is null) return false;

    var requestData = new
    {
      receiptData = appStoreNotification.Data?.SignedTransactionInfo ?? string.Empty,
      password = "ab1867a592ea4bac9f5d3193a7461ab8" // Cần lấy trong App Store Connect
    };

    using (var httpClient = new HttpClient())
    {
      var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
      var response = await httpClient.PostAsync(url, content);
      var responseString = await response.Content.ReadAsStringAsync();

      var result = JsonConvert.DeserializeObject(responseString);
      if (result is null) return false;

      return false;
    }
  }
}
