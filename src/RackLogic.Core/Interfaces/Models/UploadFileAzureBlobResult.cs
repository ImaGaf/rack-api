namespace RackLogic.Core.Interfaces.Models;
public class UploadFileAzureBlobResult
{
  public required string Url { get; set; }
  public required string FileName { get; set; }
  public required string ContentType { get; set; }
}
