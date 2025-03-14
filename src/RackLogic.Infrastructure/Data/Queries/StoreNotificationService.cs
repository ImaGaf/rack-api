using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RackLogic.Infrastructure.Configuration;
using RackLogic.Infrastructure.Token;
using RackLogic.UseCases.StoreNotifications;
using RackLogic.UseCases.StoreNotifications.AppStore;
using RackLogic.UseCases.StoreNotifications.GooglePlay;

namespace RackLogic.Infrastructure.Data.Queries;
public class StoreNotificationService(
  AppStoreConfigService _appStoreConfigService,
  GooglePlayConfigService _googlePlayConfigService,
  IConfiguration _configuration)
  : IStoreNotificationService
{
  public Task<T?> DecodeBase64Async<T>(string base64)
  {
    byte[] dataBytes = Convert.FromBase64String(base64);
    string jsonString = Encoding.UTF8.GetString(dataBytes);
    return Task.FromResult(JsonConvert.DeserializeObject<T>(jsonString ?? string.Empty));
  }

  public async Task<T?> DecodeJWTTokenAsync<T>(string token)
  {
    return await TokenUtil.DecodeJwt<T>(token);
  }

  public async Task<SignedTransactionInfoData?> VerifyAppleReceiptByTransactionIdAsync(string transactionId)
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

  public async Task<SubscriptionPurchase?> VerifyGooglePlayPurchaseTokenAsync(string? productId = "", string? token = "")
  {
    var accessToken = await TokenUtil.GenerateDeveloperGooglePlayToken(_googlePlayConfigService.GetConfig());
    var urlConfig = _configuration.GetSection("InAppPurchase:APIConfirmTransactionGooglePlay").Value ?? string.Empty;
    var packageName = _configuration.GetSection("InAppPurchase:PackageName").Value ?? string.Empty;
    var url = string.Format(urlConfig, packageName, productId, token);

    using (HttpClient client = new HttpClient())
    {
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
      var response = await client.GetAsync(url);
      if (response.IsSuccessStatusCode)
      {
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<SubscriptionPurchase>(content);
      }
    }
    return null;
  }
}
