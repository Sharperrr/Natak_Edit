using Natak.Domain;

namespace Natak.Core.Services
{
    public interface IGameStorage
    {
        Task SaveAsync(string gameId, Game game, CancellationToken cancellationToken = default);
        Task<Game?> LoadAsync(string gameId, CancellationToken cancellationToken = default);
    }
}
