using Natak.Domain;
using Natak.Infrastructure.DTOs;
using System.Text.Json;
using Natak.Core.Services;
using Natak.Infrastructure.Converters;


namespace Natak.Infrastructure
{
    public class GameStorageService : IGameStorage
    {
        private readonly string _basePath;
        private static JsonSerializerOptions JsonSerializerOptions => new()
        {
            WriteIndented = true,
            Converters = { new StateManagerDtoConverter() }
        };
        public GameStorageService()
        {
            _basePath = @"C:\SaveFolder";
        }

        private string GetFilePath(string gameId) => Path.Combine(_basePath, $"{gameId}.json");

        public async Task SaveAsync(string gameId, Game game, CancellationToken cancellationToken = default)
        {
            var dto = GameDto.FromDomain(game);
            var json = JsonSerializer.Serialize(dto, JsonSerializerOptions);
            var path = GetFilePath(gameId);
            await File.WriteAllTextAsync(path, json, cancellationToken);
        }

        public async Task<Game?> LoadAsync(string gameId, CancellationToken cancellationToken = default)
        {
            var path = GetFilePath(gameId);
            if (!File.Exists(path)) return null;

            var json = await File.ReadAllTextAsync(path, cancellationToken);
            var dto = JsonSerializer.Deserialize<GameDto>(json, JsonSerializerOptions);
            var game = dto.ToDomain();
            Console.WriteLine(json);
            return game;

        }
    }
}
