using RackLogic.UseCases.BayConfigurator;
using RackLogic.UseCases.BayConfigurator.Create;
using RackLogic.UseCases.BayConfigurator.CreateCustomComponent;
using RackLogic.UseCases.BayConfigurator.GetSWL;
using RackLogic.UseCases.BayConfigurator.Update;
using RackLogic.UseCases.BayConfigurator.UpdateSWL;
using RackLogic.Web.BayConfigurator.CreateProduct;
using RackLogic.Web.BayConfigurator.GetSWL;
using RackLogic.Web.BayConfigurator.List;
using RackLogic.Web.BayConfigurator.UpdateSWL;
using Riok.Mapperly.Abstractions;

namespace RackLogic.Web.BayConfigurator;
[Mapper(IncludedMembers = MemberVisibility.All)]
public static partial class BayConfiguratorMapper
{
  public static partial BayConfigurationCommand ToCreateBayConfigurationCommand(this Create.BayConfigurator request);
  public static partial ListBayConfigurationResponse ToBayConfigurationResponse(this BayConfigurationDTO bay);
  public static partial BayConfigurationUpdateCommand ToUpdateBayConfigurationCommand(this Update.BayConfiguratorRequest request);
  public static partial UpdateSWLCommand ToUpdateSWLCommand(this UpdateSWLRequest request);
  public static partial GetSWLCommand ToGetSWLCommand(this GetSWLRequest request);
  public static partial CreateCustomComponentCommand ToCreateProductCommand(this CreateCustomComponentRequest request);

}
