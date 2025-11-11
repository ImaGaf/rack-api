namespace RackLogic.UseCases.Common.Extensions;

public static class Base64Extension
{
  public static async Task<string> ConvertImageUrlToBase64(string imageUrl)
  {
    using var httpClient = new HttpClient();
    try
    {
      var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
      var base64 = Convert.ToBase64String(imageBytes);
      return $"data:{imageUrl?.GetContentTypeFromUrl()};base64,{base64}";
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine(ex);
      return string.Empty ;
    }
  }

  private static string GetContentTypeFromUrl(this string url)
  {
    if (url.EndsWith(".png")) return "image/png";
    if (url.EndsWith(".jpg") || url.EndsWith(".jpeg")) return "image/jpeg";
    if (url.EndsWith(".gif")) return "image/gif";
    if (url.EndsWith(".svg")) return "image/svg+xml";
    if (url.EndsWith(".webp")) return "image/webp";
    return "application/octet-stream";
  }
}
