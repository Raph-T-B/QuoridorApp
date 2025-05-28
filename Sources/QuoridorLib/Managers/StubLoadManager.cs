
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
        public List<Player> Players { get; set; } = [];
        /// <summary>
        /// Attempts to load a saved game.
        /// </summary>
        public Game LoadGame()
        {
            // This method returns a new empty game because it's a stub used only for testing.
            // In production, this method should load an existing game.
            Game game = new();
            List<Player> Players = [
                new("Jojo"),
                new("Jaja")];
            game.AddPlayer(Players[0]);
            game.AddPlayer(Players[1]);
            game.GetBestOf().AddPlayer1Victory();
            game.GetBestOf().AddPlayer2Victory();
            return game;
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

        public List<Player> LoadPlayers() 
        {
            Players = [
                new("Jojo"),
                new("Jaja"),
                new("Jiji"),
                new("Juju"),
                new("poulet"),
                new("fritesMerguez")
                ];
            return Players;


        }
    }
}
