using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Common.Enums;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.BayConfigurator.Update;
public class UpdateBayConfigurationHandler(
  IUserWarehouseService _userWarehouseService,
  IRepositoryWithExpression<BayConfiguration> _bayConfigurationRepository,
  IRepositoryWithExpression<BayConfigurationLevels> _bayConfigurationLevelsRepository,
  IRepositoryWithExpression<OEMBrand> _oEMBrandRepository,
  IRepositoryWithExpression<Component> _componentRepository)
  : ICommandHandler<UpdateBayConfigurationCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(UpdateBayConfigurationCommand request, CancellationToken cancellationToken)
  {
    if (!await _userWarehouseService.CheckPermissionUserRequestAsync(request.UserId, request.WarehouseId)) return Result.Forbidden();

    if (await CheckUniqueForBayConfigIdAsync(request.BayConfiguration.BayConfigId, request.WarehouseId, request.BayConfiguration.Id))
      return Result.Invalid(new ValidationError("BayConfigId must be unique."));

    var bayConfig = await _bayConfigurationRepository.GetByIdAsync(request.BayConfiguration.Id);
    if (bayConfig is null)
    {
      return Result.NotFound();
    }

    return await UpdateConfigurationAsync(bayConfig, request.BayConfiguration, request.WarehouseId, cancellationToken);
  }

  private async Task<bool> CheckUniqueForBayConfigIdAsync(string bayConfigId, Guid warehouseId, Guid bayId)
  {
    var result = await _bayConfigurationRepository.FirstOrDefaultAsync(x => 
      x.WarehouseId == warehouseId &&
      x.Id != bayId &&
      x.BayConfigId.Equals(bayConfigId));

    return result is not null;
  }

  private async Task<bool> UpdateConfigurationAsync(BayConfiguration bayConfig, BayConfigurationUpdateCommand request, Guid warehouseId, CancellationToken cancellationToken)
  {
    var brand = await _oEMBrandRepository.FirstOrDefaultAsync(x => x.Id == request.Manufacturer.Id);
    if (brand is null)
    {
      bayConfig.Manufacturer = request.Manufacturer.Mapping(warehouseId);
    }
    else
    {
      bayConfig.ManufacturerId = brand.Id;
    }

    bayConfig.IsCustomBrand = request.IsCustomBrand;
    bayConfig.BayConfigId = request.BayConfigId;
    bayConfig.RackingType = request.RackingType;
    bayConfig.InstallationDate = request.InstallationDate;
    bayConfig.IsEnableInterimInspection = request.IsEnableInterimInspection ?? false;
    bayConfig.ASComplianceVersion = request.AsComplianceVersion;
    bayConfig.Supplier = request.Supplier;
    bayConfig.StructuralDesigner = request.StructuralDesigner;
    bayConfig.ProjectRef = request.ProjectRef;
    bayConfig.ToleranceGrade = request.ToleranceGrade;
    bayConfig.TotalBayLoad = request.TotalBayLoad;
    bayConfig.Comment = request.Comment;
    bayConfig.IsEstimatedInstallDate = request.IsEstimatedInstallDate;
    bayConfig.InterimInterval = request.InterimInterval;
    bayConfig.HasSplice = request.HasSplice;
    bayConfig.Level = request.Level;
    bayConfig.Pallets = request.Pallets;
    bayConfig.HasProtector = request.HasProtector;
    bayConfig.SpliceKits = request.SpliceKits?.ToString();
    bayConfig.IsCustomUpRight = request.IsCustomUpRight;
    bayConfig.IsCustomBeam = request.IsCustomBeam;
    bayConfig.IsCustomBracingHorizontal = request.IsCustomBracingHorizontal;
    bayConfig.IsCustomBracingDiagonal = request.IsCustomBracingDiagonal;
    bayConfig.IsCustomRowSpacer = request.IsCustomRowSpacer;
    bayConfig.IsCustomAccessories = request.IsCustomAccessories;
    await _bayConfigurationLevelsRepository.DeleteAsync(x => x.BayId == request.Id);
    bayConfig.Components.Clear();
    bayConfig.Components.Add(request.UpRight.Mapping(bayConfig.Id));
    bayConfig.Components.Add(request.Beam.Mapping(bayConfig.Id));
    bayConfig.Components.Add(request.BracingHorizontal.Mapping(bayConfig.Id));
    bayConfig.Components.Add(request.BracingDiagonal.Mapping(bayConfig.Id));
    bayConfig.Components.Add(request.RowSpacer.Mapping(bayConfig.Id));

    if (request.Accessories != null)
    {
      bayConfig.Components.AddRange(request.Accessories.Select(c => c.Mapping(bayConfig.Id)));
    }

    await _componentRepository.DeleteAsync(x => x.BayId == request.Id);

    bayConfig.BayConfigurationLevels.Clear();

    bayConfig.BayConfigurationLevels.AddRange(
        request.BeamLevels.Select(l => l.Mapping(ComponentType.Beam, bayConfig.Id))
    );

    bayConfig.BayConfigurationLevels.AddRange(
        request.BracingHorizontalLevels.Select(l => l.Mapping(ComponentType.BraceHorizontal, bayConfig.Id))
    );

    bayConfig.BayConfigurationLevels.AddRange(
        request.RowSpacerLevels.Select(l => l.Mapping(ComponentType.RowSpacer, bayConfig.Id))
    );

    bayConfig.BayConfigurationLevels.AddRange(
        request.BracingDiagonalLevels.Select(l => l.Mapping(ComponentType.BraceDiagonal, bayConfig.Id))
    );

    await _bayConfigurationRepository.UpdateAsync(bayConfig);

    return true;
  }
}
