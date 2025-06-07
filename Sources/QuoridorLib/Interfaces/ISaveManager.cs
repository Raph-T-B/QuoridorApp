using QuoridorLib.Models;

namespace QuoridorLib.Interfaces
{
    public interface ISaveManager
    {
        public void SaveGame(Game game);
        public void SavePlayer(Player player);
    }
} 