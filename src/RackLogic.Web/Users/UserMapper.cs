using RackLogic.UseCases.Users.Create;
using RackLogic.Web.Users.Register;
using Riok.Mapperly.Abstractions;

namespace RackLogic.Web.Users;

[Mapper(IncludedMembers = MemberVisibility.All)]
public static partial class UserMapper
{
  public static partial RegisterUserCommand ToUserCommand(this RegisterUserRequest request);
}
