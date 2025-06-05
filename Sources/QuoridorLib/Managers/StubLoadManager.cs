
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
        public List<Game> Games { get; set; } = [];
        /// <summary>
        /// Attempts to load a saved game.
        /// </summary>
        public List<Game> LoadGames()
        {
            Games = [
                new (1),
                new (3),
                new (3),
                new (5),
                new (5),
                ];
            Games[0].AddPlayer(new("jojo"));
            Games[0].AddPlayer(new("mouloude"));

            Games[1].AddPlayer(new("jiji"));
            Games[1].AddPlayer(new("paulo"));
            Games[1].GetBestOf().AddPlayer1Victory();
            Games[1].GetBestOf().AddPlayer2Victory();

            Games[2].AddPlayer(new("joan"));
            Games[2].AddPlayer(new("charline"));
            Games[2].GetBestOf().AddPlayer2Victory();

            Games[3].AddPlayer(new("lecochonou"));
            Games[3].AddPlayer(new("beastrix"));
            Games[3].GetBestOf().AddPlayer2Victory();
            Games[3].GetBestOf().AddPlayer1Victory();
            Games[3].GetBestOf().AddPlayer2Victory();

            Games[4].AddPlayer(new("lecochonou"));
            Games[4].AddPlayer(new("beastrix"));
            Games[4].GetBestOf().AddPlayer2Victory();
            Games[4].GetBestOf().AddPlayer1Victory();
            Games[4].GetBestOf().AddPlayer1Victory();
            return Games;
        }

        public Game LoadGame()
        {
            Game game= new(3);
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
