namespace RackLogic.UseCases;
public class ClientOption
{
  public required string BaseUrl { get; set; }
  public string? Standard { get; set; }
  public string? LogoReport { get; set; }
  public string? LogoMarket { get; set; }
  public string LogoMarketWithUrl => new Uri(new Uri(BaseUrl), LogoMarket).ToString();
  public string LogoReportWithUrl => new Uri(new Uri(BaseUrl), LogoReport).ToString();
}
