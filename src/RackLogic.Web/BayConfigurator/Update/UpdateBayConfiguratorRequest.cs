
namespace RackLogic.Web.BayConfigurator.Update;

public class UpdateBayConfiguratorRequest
{
  public required Guid WarehouseId { get; set; }
  public required BayConfiguratorRequest BayConfigurator { get; set; }
}

public class BayConfiguratorRequest : Create.BayConfigurator
{
  public Guid Id { get; set; }
}
