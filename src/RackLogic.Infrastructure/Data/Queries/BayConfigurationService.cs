using Ardalis.Result;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases;
using RackLogic.UseCases.BayConfigurator;
using RackLogic.UseCases.BayConfigurator.ShareSWL;

namespace RackLogic.Infrastructure.Data.Queries;

public class BayConfigurationService : IBayConfigurationService
{
  private readonly IRepositoryWithExpression<BayConfiguration> _bayconfigurationRepository;
  private readonly IRepositoryWithExpression<FloorPlan> _floorPlanRepository;
  private readonly IConverter _converter;
  private readonly IEmailSender _emailSender;
  private readonly ClientOption _clientOptions;

  public BayConfigurationService(
    IRepositoryWithExpression<BayConfiguration> bayconfigurationRepository,
    IRepositoryWithExpression<FloorPlan> floorPlanRepository,
    IConverter converter,
    IEmailSender emailSender,
    IOptions<ClientOption> options)
  {
    _bayconfigurationRepository = bayconfigurationRepository;
    _floorPlanRepository = floorPlanRepository;
    _converter = converter;
    _emailSender = emailSender;
    _clientOptions = options.Value;
  }

  public async Task ExportSWLAsync(ShareSWLCommand request, CancellationToken cancellationToken = default)
  {
    var bayConfiguration = await _bayconfigurationRepository
                    .FirstOrDefaultAsync(x => x.Id == request.BayConfigurationId,
                              includes:
                                [i => i.SWLSignage,
                                 i => i.Manufacturer,
                                 i => i.Warehouses],
                              cancellationToken: cancellationToken);

    var asset = await _floorPlanRepository.FirstOrDefaultAsync(x => x.Id == request.AssetId, cancellationToken: cancellationToken);

    if (bayConfiguration is null) Result.Invalid(new ValidationError("SWL Id not found."));

    var htmlPath = Path.Combine("wwwroot", "SWLSignageTemplates", "SWLSignagePDF.html");
    var html = await File.ReadAllTextAsync(htmlPath, cancellationToken);
    var subject = "SWL Signage details have been shared with you";
    var htmlShareContent = await File.ReadAllTextAsync(Path.Combine("wwwroot", "EmailTemplates", "ShareSWL.html"));
    htmlShareContent = htmlShareContent.Replace("{logo}", _clientOptions.LogoMarketWithUrl);
    htmlShareContent = htmlShareContent.Replace("{message}", request.Message);
    htmlShareContent = htmlShareContent.Replace("{currentYear}", DateTime.UtcNow.Year.ToString());

    var pdfContent = html
      .Replace("{{RackLogicLogo}}", _clientOptions.LogoReportWithUrl)
      .Replace("{{Client}}", bayConfiguration!.Warehouses!.CompanyName)
      .Replace("{{LocationAddress}}", bayConfiguration.Warehouses.Address)
      .Replace("{{Asset}}", asset is null ? string.Empty : asset.AssetName)
      .Replace("{{InstallationDate}}", bayConfiguration.InstallationDate?.ToString("dd MMMM yyyy"))
      .Replace("{{AustralianStandardsCompliance}}", bayConfiguration.ASComplianceVersion)
      .Replace("{{Manufacturer}}", bayConfiguration.Manufacturer!.Name)
      .Replace("{{Installer}}", bayConfiguration.Supplier)
      .Replace("{{StructuralDesigner}}", bayConfiguration.StructuralDesigner)
      .Replace("{{ProjectReference}}", bayConfiguration.ProjectRef)
      .Replace("{{ToleranceGrade}}", bayConfiguration.ToleranceGrade.ToString())
      .Replace("{{PalletWeight}}", bayConfiguration.SWLSignage!.PalletWeight.ToString())
      .Replace("{{BulkPalletsNumberOfLevels}}", bayConfiguration.SWLSignage!.BulkPalletsNumberOfLevels.ToString())
      .Replace("{{BulkPalletsLevelMaximumLoad}}", bayConfiguration.SWLSignage!.BulkPalletsLevelMaximumLoad.ToString())
      .Replace("{{ShelvingNumberOfLevels}}", bayConfiguration.SWLSignage!.ShelvingNumberOfLevel.ToString())
      .Replace("{{ShelvingLevelMaximumLoad}}", bayConfiguration.SWLSignage!.ShelvingLevelMaximumLoad.ToString())
      .Replace("{{TotalBayLoad}}", bayConfiguration.TotalBayLoad.ToString())
      .Replace("{{1stBeam}}", bayConfiguration.SWLSignage!.FirstBeamLevelMeasurementFGL.ToString())
      .Replace("{{2ndBeam}}", bayConfiguration.SWLSignage!.SecondBeamLevelMeasurementFGL.ToString());

    var doc = new HtmlToPdfDocument()
    {
      Objects = {
        new ObjectSettings
        {
          HtmlContent = pdfContent,
          WebSettings = { DefaultEncoding = "utf-8" },
        }
      }
    };

    var pdfBytes = _converter.Convert(doc);

    var file = Convert.ToBase64String(pdfBytes);

    await _emailSender.SendSWLSignageAsync(file, subject, htmlShareContent, request.Email);
  }
}
