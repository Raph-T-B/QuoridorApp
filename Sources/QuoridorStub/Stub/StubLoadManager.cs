using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuoridorLib.Interfaces;
using QuoridorLib.Models;

namespace QuoridorStub.Stub
{
    /// <summary>
    /// Stub implementation of <see cref="ILoadManager"/> for testing purposes.
    /// This class does not support actual loading and throws exceptions on load attempts.
    /// </summary>
    public class StubLoadManager : ILoadManager
    {
        private  List<Player> Players = [];
         
        private  List<Game> Games = [];


        /// <summary>
        /// Attempts to load a saved game.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public Game LoadGame(int ind)
        {
            return Games[ind];
        }

        public List<Game> LoadedGames()
        {
            return Games;
        }

        public void LoadGames(List<Game> games)
        {
            Games = games;
        }

        public void AddGame(Game game)
        {
            Games.Add(game);
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }
         
        public void LoadPlayers(List<Player> players) 
        {
            Players = players;
        }
        public List<Player> LoadedPlayers()
        {
            return Players;
        }
    }
}
