using QuoridorLib.Models;

namespace QuoridorLib.Interfaces
{
    public interface ILoadManager
    {
        Board.LoadBoard();
        
        Game LoadGame();
        (Round currentRound, List<Player> players, BestOf bestOf) LoadGameState();
    }
} 