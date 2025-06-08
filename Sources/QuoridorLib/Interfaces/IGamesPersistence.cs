using QuoridorLib.Models;

namespace QuoridorLib.Interfaces
{
    public interface IGamesPersistence
    {
        void SaveGames(List<Game> games, string path);
        List<Game> LoadGames(string path);
    }

}
