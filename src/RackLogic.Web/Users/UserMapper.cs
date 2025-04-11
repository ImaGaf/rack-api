using RackLogic.UseCases.Users.Create;
using RackLogic.UseCases.Users. UpdateInspectorProfile;
using RackLogic.Web.Users.UpdateInspectorProfile;
using RackLogic.Web.Users.Register;
using Riok.Mapperly.Abstractions;
using RackLogic.UseCases.Users;
using RackLogic.Web.Users.GetInspectorProfile;

namespace RackLogic.Web.Users;

[Mapper(IncludedMembers = MemberVisibility.All)]
public static partial class UserMapper
{
  public static partial RegisterUserCommand ToUserCommand(this RegisterUserRequest request);
  public static partial UpdateInspectorProfileCommand ToInspectorProfileCommand(this UpdateInspectorProfileRequest request, string userRegisterRole);
}
