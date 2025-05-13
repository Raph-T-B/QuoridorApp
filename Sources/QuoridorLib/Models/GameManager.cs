using QuoridorLib.Interfaces;

namespace QuoridorLib.Models
{
    public class GameManager : IGameManager
    {
        private readonly ILoadManager loadManager;
        private readonly ISaveManager saveManager;
        private Game game;

        public GameManager(ILoadManager loadManager, ISaveManager saveManager)
        {
            this.loadManager = loadManager;
            this.saveManager = saveManager;
        }

        public void InitGame(Player player1, Player player2)
        {
            game = new Game();
            game.AddPlayer(player1);
            game.AddPlayer(player2);
            game.LaunchRound();
        }

        public Game LoadGame()
        {
            return loadManager.LoadGame(game);
        }

        public void PlayTurn()
        {
            if (game.IsGameOver())
                return;

            Round currentRound = game.GetCurrentRound();
            if (currentRound.IsWinnded())
            {
                game.EndGame();
                return;
            }

            currentRound.SwitchPlayer();
        }

        public bool IsGameFinished()
        {
            return game.IsGameOver();
        }
    }
} 