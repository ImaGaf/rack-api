using Ardalis.SharedKernel;

namespace RackLogic.Core.Common.Entity;
public class BaseEntityCustom : EntityBase<Guid>, IAggregateRoot
{
  public virtual bool IsDeleted { get; set; }
  public virtual Guid? DeleterId { get; set; }
  public virtual DateTime? DeletionTime { get; set; }
  public virtual DateTime? LastModificationTime { get; set; }
  public virtual Guid? LastModifierId { get; set; }
  public virtual DateTime CreationTime { get; set; } = DateTime.Now;
  public virtual Guid? CreatorId { get; set; }
}
