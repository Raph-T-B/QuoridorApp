using QuoridorLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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
            this.game = new Game();
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
            return loadManager.LoadGame();
        }

        public void PlayTurn()
        {
            if (game.IsGameOver())
                return;

            Round currentRound = game.GetCurrentRound();
            if (currentRound == null)
            {
                throw new InvalidOperationException("No round is currently active.");
            }

            Player currentPlayer = currentRound.CurrentPlayer;
            if (currentPlayer == null)
            {
                throw new InvalidOperationException("No current player in the round.");
            }

            // Trouver le prochain joueur
            List<Player> players = game.GetPlayers();
            if (players.Count != 2)
            {
                throw new InvalidOperationException("Game must have exactly 2 players.");
            }

            Player nextPlayer = players.First(p => p.Name != currentPlayer.Name);
            currentRound.SwitchCurrentPlayer(nextPlayer);
        }

        public bool IsGameFinished()
        {
            return game.IsGameOver();
        }

        public void SaveGame()
        {
            saveManager.SaveGame(game);
        }

        public Round GetCurrentRound()
        {
            return game.GetCurrentRound();
        }

        public Player GetCurrentPlayer()
        {
            return game.GetCurrentPlayer();
        }

        public List<Player> GetPlayers()
        {
            return game.GetPlayers();
        }

        public BestOf GetBestOf()
        {
            return game.GetBestOf();
        }
    }
} 