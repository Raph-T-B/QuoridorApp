using QuoridorLib.Models;

namespace QuoridorLib.Interfaces
{
    public interface ISaveManager
    {
        void SaveGame(Game game);
        void SavePlayer(Player player);
    }
} 