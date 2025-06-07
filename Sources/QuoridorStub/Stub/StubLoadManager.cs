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
        private readonly List<Player> Players = [];
         
        private readonly List<Game> Games = [];


        /// <summary>
        /// Attempts to load a saved game.
        /// </summary>
        /// <returns>
        /// Throws <see cref="NotSupportedException"/> because this stub does not implement game loading.
        /// </returns>
        public Game LoadGame(int ind)
        {
            return Games[ind];
        }

        public List<Game> LoadedGames()
        {
            return Games;
        }

        public void AddGame(Game game)
        {
            Games.Add(game);
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }
         
        public List<Player> LoadPlayers() 
        {
            Players.Add(new("Jojo"));
            Players.Add(new("Jaja"));
            Players.Add(new("Jiji"));
            Players.Add(new("Juju"));
            Players.Add(new("poulet"));
            Players.Add(new("fritesMerguez"));
            return Players;


        }
    }
}
