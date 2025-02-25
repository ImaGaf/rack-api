using System.Net.Http.Headers;
using RackLogic.Infrastructure.Token;
using RackLogic.UseCases.StoreNotifications;

namespace RackLogic.Infrastructure.Data.Queries;
public class StoreNotificationService : IStoreNotificationService
{
  public async Task<T?> DecodeJWTToken<T>(string token)
  {
    return await TokenUtil.DecodeJwt<T>(token);
  }

  public async Task<bool> VerifyAppleReceiptByTransactionId(string transactionId)
  {
    string url = $"https://api.storekit-sandbox.itunes.apple.com/inApps/v1/transactions/{transactionId}";

    using (var client = new HttpClient())
    {
      var developerToken = TokenUtil.GenerateDeveloperIOSToken(keyId: "6ND3L2K6NK", issuerId: "a8aabbe6-a8ff-43b2-a45f-42644313fa3b", bundleId: "com.at.rack.logic");
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", developerToken);
      var response = await client.GetAsync(url);

      if (response.IsSuccessStatusCode)
      {
        var rs = await response.Content.ReadAsStringAsync();
        return true;
      }
      else
      {
        throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
      }
    }
  }
}
