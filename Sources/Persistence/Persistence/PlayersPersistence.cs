using System.Text.Json;
using QuoridorLib.Interfaces;
using QuoridorLib.Models;

namespace Persistence.Persistence
{
    public class PlayersPersistence : IPlayersPersistence
    {
        public List<Player> LoadPlayers(string path)
        {
            if (!File.Exists(path))
                return [];

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Player>>(json) ?? [];
        }

        public void SavePlayers(List<Player> players, string path)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true // JSON lisible
            };

            string json = JsonSerializer.Serialize(players, options);
            File.WriteAllText(path, json);
        }
    }
}
