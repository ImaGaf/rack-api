using Microsoft.AspNetCore.Identity;

namespace RackLogic.Core.Common.Entity;

public class RolePermission : BaseEntityCustom
{
  public Guid RoleId { get; set; }
  public string? Code { get; set; }

  public virtual IdentityRole<Guid>? Role { get; set; }
}
