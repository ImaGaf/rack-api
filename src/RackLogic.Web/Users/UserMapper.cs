using Riok.Mapperly.Abstractions;
using RackLogic.UseCases.Users.Create;
using RackLogic.Web.Users.Register;

namespace RackLogic.Web.Users;

[Mapper(IncludedMembers = MemberVisibility.All)]
public static partial class UserMapper
{
  public static partial RegisterUserCommand ToUserCommand(this RegisterUserRequest request);
}
