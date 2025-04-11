using System.Security.Claims;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace RackLogic.UseCases.Users.Get;
public record GetUserQuery()
  : IQuery<Result<UserDTO>>;
