using System.Text.Json;
using QuoridorLib.Interfaces;
using QuoridorLib.Models;
using Persistence.Persistence;

namespace Persistence.Persistence
{
    public class GamePersistence : IGamesPersistence
    {
        public List<Game> LoadGames(string path)
        {
            if (!File.Exists(path))
                return [];

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Game>>(json) ?? [];
        }

        public void SaveGames(List<Game> games, string path)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true // Pour rendre le JSON lisible
            };

            string json = JsonSerializer.Serialize(games, options);
            File.WriteAllText(path, json);
        }
    }
}
