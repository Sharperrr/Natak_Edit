using Natak.Core.Abstractions;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.SaveLoad;

internal sealed class SaveLoadCommandHandler(IActiveGameCache cache, IGameStorage storage) :
    ICommandHandler<SaveLoadCommand>
{
    public async Task<Result> Handle(SaveLoadCommand request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        if (!request.IsSaved)
        {
            await storage.SaveAsync(request.GameId, game, cancellationToken);
        }
        else
        {
            var restoredGame = await storage.LoadAsync(request.GameId, cancellationToken);
            if (restoredGame is null)
                return Result.Failure(GameErrors.GameNotFound);

            await cache.UpsetAsync(request.GameId, restoredGame, cancellationToken: cancellationToken);
        }

        return Result.Success();
    }
}
