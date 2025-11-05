namespace RackLogic.UseCases.Warehouses;
public class WarehousesDTO
{
  public Guid WarehouseId { get; set; }
  public string? Name{ get; set; }
  public string? FilePathImage { get; set; }
  public bool IsOwner { get; set; }
}
