using QuoridorLib.Models;

namespace QuoridorLib.Interfaces
{
    public interface ILoadManager
    {
        Game LoadGame();
        (Round currentRound, List<Player> players, BestOf bestOf) LoadGameState();
    }
} 