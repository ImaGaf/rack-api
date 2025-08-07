namespace RackLogic.UseCases.Common.Extensions;

public static class Base64Extension
{
  public static async Task<string> ConvertImageUrlToBase64(string imageUrl)
  {
    using var httpClient = new HttpClient();
    var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

    var contentType = GetContentTypeFromUrl(imageUrl);
    var base64 = Convert.ToBase64String(imageBytes);

    return $"data:{contentType};base64,{base64}";
  }

  private static string GetContentTypeFromUrl(string url)
  {
    if (url.EndsWith(".png")) return "image/png";
    if (url.EndsWith(".jpg") || url.EndsWith(".jpeg")) return "image/jpeg";
    if (url.EndsWith(".gif")) return "image/gif";
    if (url.EndsWith(".svg")) return "image/svg+xml";
    if (url.EndsWith(".webp")) return "image/webp";
    return "application/octet-stream";
  }
}
