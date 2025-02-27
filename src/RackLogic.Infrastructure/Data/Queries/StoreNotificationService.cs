using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RackLogic.Infrastructure.Configuration;
using RackLogic.Infrastructure.Token;
using RackLogic.UseCases.StoreNotifications;
using RackLogic.UseCases.StoreNotifications.AppStore;

namespace RackLogic.Infrastructure.Data.Queries;
public class StoreNotificationService(
  AppStoreConfigService _appStoreConfigService,
  IConfiguration _configuration)
  : IStoreNotificationService
{
  public async Task<T?> DecodeJWTToken<T>(string token)
  {
    return await TokenUtil.DecodeJwt<T>(token);
  }

  public async Task<SignedTransactionInfoData?> VerifyAppleReceiptByTransactionId(string transactionId)
  {
    var urlConfig = _configuration.GetSection("InAppPurchase:APIConfirmTransactionAppStore").Value ?? string.Empty;
    var url = string.Format(urlConfig, transactionId);

    using (var client = new HttpClient())
    {
      var developerToken = TokenUtil.GenerateDeveloperIOSToken(_appStoreConfigService.GetConfig());
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", developerToken);
      var response = await client.GetAsync(url);

      if (response.IsSuccessStatusCode)
      {
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<SignedTransactionInfoData>(content);
      }
      else
      {
        throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
      }
    }
  }
}
