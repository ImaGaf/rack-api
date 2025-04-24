using Ardalis.Result;
using Ardalis.SharedKernel;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Common.Enums;
using RackLogic.Core.Interfaces;
using RackLogic.UseCases.Warehouses;

namespace RackLogic.UseCases.BayConfigurator.Create;
public class CreateBayConfigurationHandler(
  IUserWarehouseService _userWarehouseService,
  IRepositoryWithExpression<BayConfiguration> _bayConfigurationRepository,
  IRepositoryWithExpression<OEMBrand> _oEMBrandRepository)
  : ICommandHandler<CreateBayConfigurationCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(CreateBayConfigurationCommand request, CancellationToken cancellationToken)
  {
    if (!await _userWarehouseService.CheckPermissionUserRequestAsync(request.UserId, request.WarehouseId)) return Result.Forbidden();

    if (await CheckUniqueForBayConfigIdAsync(request.BayConfiguration.BayConfigId, request.WarehouseId)) return Result.Invalid(new ValidationError("BayConfigId must be unique."));

    return await AddConfigurationAsync(request, cancellationToken);
  }

  private async Task<bool> CheckUniqueForBayConfigIdAsync(string bayConfigId, Guid warehouseId)
  {
    var result = await _bayConfigurationRepository.FirstOrDefaultAsync(x => x.WarehouseId == warehouseId && x.BayConfigId.Equals(bayConfigId));

    return result is not null;
  }

  private async Task<bool> AddConfigurationAsync(CreateBayConfigurationCommand request, CancellationToken cancellationToken)
  {
    var brand = await _oEMBrandRepository.FirstOrDefaultAsync(x => x.Id == request.BayConfiguration.Manufacturer.Id);
    var bayConfiguration = request.BayConfiguration.Mapping(request.WarehouseId);

    // Add Manufacturer to Bay Configuration
    if (brand is null)
    {
      bayConfiguration.Manufacturer = request.BayConfiguration.Manufacturer.Mapping(request.WarehouseId);
    }
    else
    {
      bayConfiguration.ManufacturerId = brand.Id;
    }
    // Add Components to BayConfiguration
    bayConfiguration.Components.Add(request.BayConfiguration.UpRight.Mapping(bayConfiguration.Id));
    bayConfiguration.Components.Add(request.BayConfiguration.Beam.Mapping(bayConfiguration.Id));
    bayConfiguration.Components.Add(request.BayConfiguration.BracingHorizontal.Mapping(bayConfiguration.Id));
    bayConfiguration.Components.Add(request.BayConfiguration.BracingDiagonal.Mapping(bayConfiguration.Id));
    bayConfiguration.Components.Add(request.BayConfiguration.RowSpacer.Mapping(bayConfiguration.Id));

    if (request.BayConfiguration.Accessories != null)
    {
      bayConfiguration.Components.AddRange(request.BayConfiguration.Accessories.Select(c => c.Mapping(bayConfiguration.Id)));
    }

    if (request.BayConfiguration.SpliceKits != null)
    {
      bayConfiguration.Components.Add(request.BayConfiguration.SpliceKits.Mapping(bayConfiguration.Id));
    }

    // Add Levels to BayConfiguration
    bayConfiguration.BayConfigurationLevels.AddRange(
        request.BayConfiguration.BeamLevels.Select(l => l.Mapping(ComponentType.Beam, bayConfiguration.Id))
    );

    bayConfiguration.BayConfigurationLevels.AddRange(
        request.BayConfiguration.BracingHorizontalLevels.Select(l => l.Mapping(ComponentType.BraceHorizontal, bayConfiguration.Id))
    );

    bayConfiguration.BayConfigurationLevels.AddRange(
        request.BayConfiguration.RowSpacerLevels.Select(l => l.Mapping(ComponentType.Backtie, bayConfiguration.Id))
    );
    bayConfiguration.BayConfigurationLevels.AddRange(
        request.BayConfiguration.BracingDiagonalLevels.Select(l => l.Mapping(ComponentType.BraceDiagonal, bayConfiguration.Id))
    );

    bayConfiguration.SWLSignage = new SWLSignage() {  BayConfigurationId = bayConfiguration.Id};

    var result = await _bayConfigurationRepository.AddAsync(bayConfiguration, cancellationToken);

    return result is not null;
  }
}
