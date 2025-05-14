using QuoridorLib.Models;

namespace QuoridorLib.Interfaces
{
    public interface ILoadManager
    {
        Game LoadGame();
        GameState LoadGameState();
    }

    public class GameState
    {
        public Round? CurrentRound { get; set; }
        public List<Player> Players { get; set; } = new();
        public BestOf BestOf { get; set; } = new(3);
    }
} 