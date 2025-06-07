using QuoridorLib.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace QuoridorLib.Interfaces
{
    public interface ILoadManager
    {
        public Game LoadGame(int ind);
        public List<Game> LoadedGames();
        public void LoadGames(List<Game> games);
        public void AddGame(Game game);
        public void AddPlayer(Player player);
        public void LoadPlayers(List<Player> players);
        public List<Player> LoadedPlayers();
    }

    public class GameState
    {
        public Round? CurrentRound { get; set; }
        public ReadOnlyCollection<Player> Players { get; set; } = new List<Player>().AsReadOnly();
        public BestOf BestOf { get; set; } = new(3);
    }
} 