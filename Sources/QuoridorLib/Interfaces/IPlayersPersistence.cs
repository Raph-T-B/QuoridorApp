using QuoridorLib.Models;

namespace QuoridorLib.Interfaces
{
     public interface IPlayersPersistence
    {
        void SavePlayers(List<Player> players, string path);
        List<Player> LoadPlayers(string path);
    }
}
