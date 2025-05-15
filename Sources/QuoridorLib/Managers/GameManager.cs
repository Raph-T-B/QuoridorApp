using QuoridorLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace QuoridorLib.Models
{
    public class GameManager : IGameManager
    {
        private readonly ILoadManager loadManager;
        private readonly ISaveManager saveManager;
        private Game game;

        // Définition des événements
        public event EventHandler<(Player player1, Player player2)> GameInitialized = delegate { };
        public event EventHandler<Player> TurnStarted = delegate { };
        public event EventHandler<Player> TurnEnded = delegate { };
        public event EventHandler<BestOf> GameFinished = delegate { };
        public event EventHandler<GameState> GameStateChanged = delegate { };

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
            GameInitialized(this, (player1, player2));
            GameStateChanged(this, new GameState
            {
                CurrentRound = game.GetCurrentRound(),
                Players = game.GetPlayers(),
                BestOf = game.GetBestOf()
            });
        }

        public void PlayTurn()
        {
            if (game.IsGameOver())
                return;

            Round? currentRound = game.GetCurrentRound();
            if (currentRound == null)
            {
                throw new InvalidOperationException("No round is currently active.");
            }

            Player? currentPlayer = currentRound.CurrentPlayerProperty;
            if (currentPlayer == null)
            {
                throw new InvalidOperationException("No current player in the round.");
            }

            TurnStarted(this, currentPlayer);

            var players = game.GetPlayers();
            if (players.Count != 2)
            {
                throw new InvalidOperationException("Game must have exactly 2 players.");
            }

            Player nextPlayer = players.First(p => p.Name != currentPlayer.Name);
            currentRound.SwitchCurrentPlayer(nextPlayer);

            TurnEnded(this, nextPlayer);
            GameStateChanged(this, new GameState
            {
                CurrentRound = game.GetCurrentRound(),
                Players = game.GetPlayers(),
                BestOf = game.GetBestOf()
            });
        }

        public bool IsGameFinished()
        {
            bool isFinished = game.IsGameOver();
            if (isFinished)
            {
                GameFinished(this, game.GetBestOf());
            }
            return isFinished;
        }

        public Game LoadGame()
        {
            return loadManager.LoadGame();
        }

        public void SaveGame()
        {
            saveManager.SaveGame(game);
        }

        public Round? GetCurrentRound()
        {
            return game.GetCurrentRound();
        }

        public Player? GetCurrentPlayer()
        {
            return game.GetCurrentPlayer();
        }

        public ReadOnlyCollection<Player> GetPlayers()
        {
            return game.GetPlayers();
        }

        public BestOf GetBestOf()
        {
            return game.GetBestOf();
        }

        public void SaveGameState()
        {
            var gameState = new GameState
            {
                CurrentRound = game.GetCurrentRound(),
                Players = game.GetPlayers(),
                BestOf = game.GetBestOf()
            };
            saveManager.SaveGameState(gameState);
            GameStateChanged(this, gameState);
        }

        public void LoadGameState()
        {
            var gameState = loadManager.LoadGameState();
            if (gameState.CurrentRound != null)
            {
                game = new Game();
                foreach (var player in gameState.Players)
                {
                    game.AddPlayer(player);
                }
                GameStateChanged(this, gameState);
            }
        }
    }
} 