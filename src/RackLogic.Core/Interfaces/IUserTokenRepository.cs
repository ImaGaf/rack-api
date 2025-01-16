namespace RackLogic.Core.Interfaces;
public interface IUserTokenRepository
{
  Task DeleteTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
