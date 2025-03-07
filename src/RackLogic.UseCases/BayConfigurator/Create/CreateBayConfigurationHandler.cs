using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using RackLogic.Core.Common.Constants;
using RackLogic.Core.Common.Entity;
using RackLogic.Core.Common.Enums;
using RackLogic.Core.Interfaces;

namespace RackLogic.UseCases.BayConfigurator.Create;
public class CreateBayConfigurationHandler(
  IRepositoryWithExpression<UserWarehouseRole> _userWarehouseRoleRepository,
  IRepositoryWithExpression<BayConfiguration> _bayConfigurationRepository,
  IRepositoryWithExpression<Component> _componentRepository,
  IRepositoryWithExpression<BayConfigurationLevels> _bayConfigurationLevelsRepository,
  RoleManager<IdentityRole<Guid>> _roleManager) : ICommandHandler<CreateBayConfigurationCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(CreateBayConfigurationCommand request, CancellationToken cancellationToken)
  {
    if (!await CheckPermissionUserRequest(request.UserId, request.WarehouseId)) return Result.Forbidden();

    return await AddConfigurationAsync(request, cancellationToken);
  }

  private async Task<bool> CheckPermissionUserRequest(Guid userId, Guid warehouseId)
  {
    var result = await _userWarehouseRoleRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.WarehouseId == warehouseId);
    if (result == null) return false;

    var role = await _roleManager.FindByIdAsync(result.RoleId.ToString());
    if (role == null) return false;

    if (role.Name != RoleConstant.Premises &&
      role.Name != RoleConstant.ClientManager &&
      role.Name != RoleConstant.Inspector) return false;

    return true;
  }

  private async Task<bool> AddConfigurationAsync(CreateBayConfigurationCommand request, CancellationToken cancellationToken)
  {
    var bayConfiguration = request.BayConfiguration.Mapping(request.WarehouseId);
    var bayConfigurationResult = await _bayConfigurationRepository.AddAsync(bayConfiguration, cancellationToken);

    if (bayConfigurationResult == null) return false;

    // add components to DB
    var components = new List<Component>
        {
            request.BayConfiguration.UpRight.Mapping(bayConfiguration.Id),
            request.BayConfiguration.Beam.Mapping(bayConfiguration.Id),
            request.BayConfiguration.BracingHorizontal.Mapping(bayConfiguration.Id),
            request.BayConfiguration.BracingDiagonal.Mapping(bayConfiguration.Id),
            request.BayConfiguration.RowSpacer.Mapping(bayConfiguration.Id)
        };

    if (request.BayConfiguration.Accessories.BasePlate != null)
    {
      components.AddRange(request.BayConfiguration.Accessories.BasePlate.Select(c => c.Mapping(bayConfiguration.Id)));
    }

    var componentResult = await _componentRepository.AddRangeAsync(components, cancellationToken);
    if (componentResult == null) return false;

    // Add levels to DB
    var bayLevels = request.BayConfiguration.BeamLevels.Select(l => l.Mapping(ComponentType.Beam, bayConfiguration.Id)).ToList();
    bayLevels.AddRange(request.BayConfiguration.BracingHorizontalLevels.Select(l => l.Mapping(ComponentType.BracingHorizontal, bayConfiguration.Id)));
    bayLevels.AddRange(request.BayConfiguration.RowSpacerLevels.Select(l => l.Mapping(ComponentType.RowSpacer, bayConfiguration.Id)));
    bayLevels.AddRange(request.BayConfiguration.BracingDiagonalLevels.Select(l => l.Mapping(ComponentType.BracingDiagonal, bayConfiguration.Id)));
    var levelResult = await _bayConfigurationLevelsRepository.AddRangeAsync(bayLevels, cancellationToken);

    return levelResult is not null;
  }
}
