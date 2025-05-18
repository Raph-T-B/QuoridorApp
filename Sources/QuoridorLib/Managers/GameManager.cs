using QuoridorLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace QuoridorLib.Models
{
    /// <summary>
    /// Main game manager for the Quoridor game.
    /// Manages game initialization, turn progression, saving/loading,
    /// and raising events related to game state changes.
    /// </summary>
    public class GameManager : IGameManager
    {
        private readonly ILoadManager loadManager;
        private readonly ISaveManager saveManager;
        private Game game;

        /// <summary>
        /// Event triggered when a new game is initialized.
        /// Provides the two players participating in the game.
        /// </summary>
        public event EventHandler<(Player player1, Player player2)> GameInitialized = delegate { };

        /// <summary>
        /// Event triggered at the start of a player's turn.
        /// Provides the player whose turn it is.
        /// </summary>
        public event EventHandler<Player> TurnStarted = delegate { };

        /// <summary>
        /// Event triggered at the end of a player's turn.
        /// Provides the player who just finished their turn.
        /// </summary>
        public event EventHandler<Player> TurnEnded = delegate { };

        /// <summary>
        /// Event triggered when the game finishes.
        /// Provides the BestOf object containing the final score.
        /// </summary>
        public event EventHandler<BestOf> GameFinished = delegate { };

        /// <summary>
        /// Event triggered whenever there is a significant change in the game state.
        /// Provides a GameState object with current information.
        /// </summary>
        public event EventHandler<GameState> GameStateChanged = delegate { };

        /// <summary>
        /// Initializes a new instance of the <see cref="GameManager"/> class.
        /// </summary>
        /// <param name="loadManager">The manager responsible for loading saved games.</param>
        /// <param name="saveManager">The manager responsible for saving games.</param>
        public GameManager(ILoadManager loadManager, ISaveManager saveManager)
        {
            this.loadManager = loadManager;
            this.saveManager = saveManager;
            this.game = new Game();
        }

        /// <summary>
        /// Initializes a new game with two players.
        /// Starts the first round and triggers the <see cref="GameInitialized"/> event.
        /// </summary>
        /// <param name="player1">The first player.</param>
        /// <param name="player2">The second player.</param>
        public void InitGame(Player player1, Player player2, int numberOfGames = 3)
        {
            var existingBestOf = game.GetBestOf();
            game = new Game(numberOfGames);
            game.AddPlayer(player1);
            game.AddPlayer(player2);
            game.LaunchRound();
            // Restaurer le BestOf existant
            var newBestOf = game.GetBestOf();
            for (int i = 0; i < existingBestOf.GetPlayer1Score(); i++)
            {
                newBestOf.AddPlayer1Victory();
            }
            for (int i = 0; i < existingBestOf.GetPlayer2Score(); i++)
            {
                newBestOf.AddPlayer2Victory();
            }
            GameInitialized(this, (player1, player2));
            GameStateChanged(this, new GameState
            {
                CurrentRound = game.GetCurrentRound(),
                Players = game.GetPlayers(),
                BestOf = game.GetBestOf()
            });
        }

        /// <summary>
        /// Executes the current player's turn.
        /// Switches the current player and raises events for turn start and end.
        /// Updates the game state via <see cref="GameStateChanged"/>.
        /// </summary>
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

        /// <summary>
        /// Checks if the game has finished.
        /// If so, triggers the <see cref="GameFinished"/> event.
        /// </summary>
        /// <returns>True if the game is finished; otherwise, false.</returns>
        public bool IsGameFinished()
        {
            bool isFinished = game.IsGameOver();
            if (isFinished)
            {
                GameFinished(this, game.GetBestOf());
            }
            return isFinished;
        }

        /// <summary>
        /// Loads a saved game using the load manager.
        /// </summary>
        /// <returns>The loaded <see cref="Game"/> instance.</returns>
        public Game LoadGame()
        {
            return loadManager.LoadGame();
        }

        /// <summary>
        /// Saves the current game using the save manager.
        /// </summary>
        public void SaveGame()
        {
            saveManager.SaveGame(game);
        }

        /// <summary>
        /// Gets the current active round.
        /// </summary>
        /// <returns>The current round, or null if none is active.</returns>
        public Round? GetCurrentRound()
        {
            return game.GetCurrentRound();
        }

        /// <summary>
        /// Gets the player whose turn it currently is.
        /// </summary>
        /// <returns>The current player, or null if none.</returns>
        public Player? GetCurrentPlayer()
        {
            return game.GetCurrentPlayer();
        }

        /// <summary>
        /// Gets the list of players participating in the game.
        /// </summary>
        /// <returns>A read-only collection of players.</returns>
        public ReadOnlyCollection<Player> GetPlayers()
        {
            return game.GetPlayers();
        }

        /// <summary>
        /// Gets the <see cref="BestOf"/> object containing the score and number of games.
        /// </summary>
        /// <returns>The current BestOf instance.</returns>
        public BestOf GetBestOf()
        {
            return game.GetBestOf();
        }

        /// <summary>
        /// Saves the complete game state (round, players, score).
        /// Triggers the <see cref="GameStateChanged"/> event.
        /// </summary>
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

        /// <summary>
        /// Loads the complete game state from a saved file.
        /// Initializes a new game with loaded players and triggers <see cref="GameStateChanged"/>.
        /// </summary>
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
