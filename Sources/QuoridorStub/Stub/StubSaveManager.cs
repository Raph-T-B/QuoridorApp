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
    /// Stub implementation of <see cref="ISaveManager"/> for testing purposes.
    /// This class does not support actual saving and throws exceptions on save attempts.
    /// </summary>
    public class StubSaveManager : ISaveManager
    {
        private readonly StubLoadManager _loadManager;


        public StubSaveManager(StubLoadManager loadManager)
        {
            _loadManager = loadManager;
        }

        /// <summary>
        /// Attempts to save the current game.
        /// </summary>
        /// <param name="game">The game instance to save.</param>
        /// <exception cref="NotSupportedException">Thrown because save is not implemented in the stub.</exception>
        public void SaveGame(Game game)
        {
            _loadManager.AddGame(game);
        }

        public void SavePlayer(Player player)
        {
            _loadManager.AddPlayer(player);
        }


    }
}
