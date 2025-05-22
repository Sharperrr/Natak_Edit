using Natak.Core.Abstractions;

namespace Natak.Core.GameActions.SaveLoad;

public sealed record SaveLoadCommand(string GameId, bool IsSaved) : ICommand;
