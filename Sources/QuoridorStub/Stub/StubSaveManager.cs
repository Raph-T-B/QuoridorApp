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
        /// <summary>
        /// Attempts to save the current game.
        /// </summary>
        /// <param name="game">The game instance to save.</param>
        /// <exception cref="NotSupportedException">Thrown because save is not implemented in the stub.</exception>
        public void SaveGame(Game game)
        {
            // This method is intentionally empty because it's a stub used only for testing.
            // In production, this method should save the state of the game.
            throw new NotSupportedException("SaveGame is not implemented in the stub.");
        }

        /// <summary>
        /// Attempts to save the complete state of the game.
        /// </summary>
        /// <param name="gameState">The game state to save.</param>
        /// <exception cref="NotSupportedException">Thrown because save is not implemented in the stub.</exception>
        public void SaveGameState(GameState gameState)
        {
            // This method is intentionally empty because it's a stub used only for testing.
            // In production, this method should save the complete state of the game.
            throw new NotSupportedException("SaveGameState is not implemented in the stub.");
        }
    }
}
