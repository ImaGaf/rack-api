namespace RackLogic.Web.Warehouses.GetListWarehouses;

public class GetListWarehousesResponse
{
  public Guid WarehouseId { get; set; }
  public string? Name { get; set; }
  public string? FilePathImage { get; set; }
  public bool IsOwner { get; set; }
}
