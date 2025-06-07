using QuoridorLib.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace QuoridorLib.Interfaces
{
    public interface ILoadManager
    {
        Game LoadGame(int ind);
        List<Player> LoadPlayers();
    }

    public class GameState
    {
        public Round? CurrentRound { get; set; }
        public ReadOnlyCollection<Player> Players { get; set; } = new List<Player>().AsReadOnly();
        public BestOf BestOf { get; set; } = new(3);
    }
} 