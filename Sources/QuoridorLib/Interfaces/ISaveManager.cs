using QuoridorLib.Models;

namespace QuoridorLib.Interfaces
{
    public interface ISaveManager
    {
        public void SaveGame(Game game);

        public void SavePlayer(Player player);

        public List<Game> GamesToSave();

        public List<Player> PlayerstoSave();
    }
} 