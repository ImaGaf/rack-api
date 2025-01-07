using RackLogic.UseCases.FloorPlans;
using RackLogic.UseCases.FloorPlans.Create;
using RackLogic.UseCases.FloorPlans.Update;
using RackLogic.Web.FloorPlans.Create;
using RackLogic.Web.FloorPlans.Get;
using RackLogic.Web.FloorPlans.Update;
using Riok.Mapperly.Abstractions;

namespace RackLogic.Web.FloorPlans;

[Mapper(IncludedMembers = MemberVisibility.All)]
public static partial class FloorPlanMapper
{
  public static partial FloorPlanCommand ToFloorPlanCommand(this FloorPlanRequest request);
  public static partial UpdateFloorPlanInformationCommand ToUpdateFloorPlanCommand(this UpdateFloorPlanInformation request);
  public static partial GetFloorPlanByIdResponse ToGetFloorPlanByIdResponse(this FloorPlanDTO reqest);
}
