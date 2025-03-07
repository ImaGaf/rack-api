using RackLogic.UseCases.BayConfigurator;
using RackLogic.UseCases.BayConfigurator.Create;
using RackLogic.Web.BayConfigurator.List;
using Riok.Mapperly.Abstractions;

namespace RackLogic.Web.BayConfigurator;
[Mapper(IncludedMembers = MemberVisibility.All)]
public static partial class BayConfiguratorMapper
{
  public static partial BayConfigurationCommand ToCreateBayConfigurationCommand(this Create.BayConfigurator request);
  public static partial ListBayConfigurationResponse ToBayConfigurationResponse(this BayConfigurationDTO bay);
}
