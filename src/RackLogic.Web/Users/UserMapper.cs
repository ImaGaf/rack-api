using RackLogic.UseCases.Users.Create;
using RackLogic.UseCases.Users.UpdateInspectorProfile;
using RackLogic.Web.Users.Register;
using RackLogic.Web.Users.UpdateInspectorProfile;
using Riok.Mapperly.Abstractions;

namespace RackLogic.Web.Users;

[Mapper(IncludedMembers = MemberVisibility.All)]
public static partial class UserMapper
{
  public static partial RegisterUserCommand ToUserCommand(this RegisterUserRequest request);
  public static partial UpdateInspectorProfileCommand ToInspectorProfileCommand(this UpdateInspectorProfileRequest request, string userRegisterRole);
}
