namespace RackLogic.UseCases.Users;
public interface IUserService
{
  Task<bool> IsPaid();
  Task<bool> IsTransactionGenerated();
}
