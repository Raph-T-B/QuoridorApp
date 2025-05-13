using QuoridorLib.Models;

namespace QuoridorLib.Interfaces
{
    public interface ISaveManager
    {
        void SaveGame(Game game);
        void SaveGameState(Round currentRound, List<Player> players, BestOf bestOf);
    }
} 