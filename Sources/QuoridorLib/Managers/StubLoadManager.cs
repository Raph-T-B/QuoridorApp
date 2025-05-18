using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuoridorLib.Interfaces;
using QuoridorLib.Models;

namespace QuoridorLib.Managers
{
    /// <summary>
    /// Stub implementation of <see cref="ILoadManager"/> for testing purposes.
    /// This class does not support actual loading and throws exceptions on load attempts.
    /// </summary>
    public class StubLoadManager : ILoadManager
    {
        /// <summary>
        /// Attempts to load a saved game.
        /// </summary>
        /// <returns>
        /// Throws <see cref="NotSupportedException"/> because this stub does not implement game loading.
        /// </returns>
        public Game LoadGame()
        {
            // This method returns a new empty game because it's a stub used only for testing.
            // In production, this method should load an existing game.
            throw new NotSupportedException("LoadGame is not implemented in the stub.");
        }

        /// <summary>
        /// Attempts to load a saved game state.
        /// </summary>
        /// <returns>
        /// Throws <see cref="NotSupportedException"/> because this stub does not implement game state loading.
        /// </returns>
        public GameState LoadGameState()
        {
            // This method returns a new empty state because it's a stub used only for testing.
            // In production, this method should load the state of an existing game.
            throw new NotSupportedException("LoadGameState is not implemented in the stub.");
        }
    }
}
