using QuoridorLib.Interfaces;
using QuoridorLib.Models;
using System.Collections.ObjectModel;

namespace QuoridorLib.Managers;

/// <summary>
/// Main game manager for the Quoridor game.
/// Manages game initialization, turn progression, saving/loading,
/// and raising events related to game state changes.
/// </summary>
public class GameManager : IGameManager
{
    private readonly ILoadManager LoadManager;
    private readonly ISaveManager SaveManager;
    private Game Game;

    /// <summary>
    /// Event triggered when a new Game is initialized.
    /// Provides the two players participating in the Game.
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
    /// Event triggered when the Game finishes.
    /// Provides the BestOf object containing the final score.
    /// </summary>
    public event EventHandler<BestOf> GameFinished = delegate { };

    /// <summary>
    /// Event triggered whenever there is a significant change in the Game state.
    /// Provides a GameState object with current information.
    /// </summary>
    public event EventHandler<GameState> GameStateChanged = delegate { };

    /// <summary>
    /// Initializes a new instance of the <see cref="GameManager"/> class.
    /// </summary>
    /// <param name="loadManager">The manager responsible for loading saved Games.</param>
    /// <param name="saveManager">The manager responsible for saving Games.</param>
    public GameManager(ILoadManager loadManager,ISaveManager saveManager)
    {
        LoadManager = loadManager;
        SaveManager = saveManager;
        Game = new Game();
    }

    /// <summary>
    /// Initializes a new Game with two players.
    /// Starts the first round and triggers the <see cref="GameInitialized"/> event.
    /// </summary>
    /// <param name="player1">The first player.</param>
    /// <param name="player2">The second player.</param>
    public void InitGame(Player player1, Player player2, int numberOfGames = 3)
    {
        var existingBestOf = Game.GetBestOf();
        Game = new Game(numberOfGames);
        Game.AddPlayer(player1);
        Game.AddPlayer(player2);
        Game.LaunchRound();
        // Restaurer le BestOf existant
        var newBestOf = Game.GetBestOf();
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
            CurrentRound = Game.GetCurrentRound(),
            Players = Game.GetPlayers(),
            BestOf = Game.GetBestOf()
        });
    }

    /// <summary>
    /// Executes the current player's turn.
    /// Switches the current player and raises events for turn start and end.
    /// Updates the Game state via <see cref="GameStateChanged"/>.
    /// </summary>
    public void PlayTurn()
    {
        if (Game.IsGameOver())
            return;

        Round? currentRound = Game.GetCurrentRound();
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

        var players = Game.GetPlayers();
        if (players.Count != 2)
        {
            throw new InvalidOperationException("Game must have exactly 2 players.");
        }

        Player nextPlayer = players.First(p => p.Name != currentPlayer.Name);
        currentRound.SwitchCurrentPlayer(nextPlayer);

        TurnEnded(this, nextPlayer);
        GameStateChanged(this, new GameState
        {
            CurrentRound = Game.GetCurrentRound(),
            Players = Game.GetPlayers(),
            BestOf = Game.GetBestOf()
        });
    }

    /// <summary>
    /// Checks if the game has finished.
    /// If so, triggers the <see cref="GameFinished"/> event.
    /// </summary>
    /// <returns>True if the game is finished; otherwise, false.</returns>
    public bool IsGameFinished()
    {
        bool isFinished = Game.IsGameOver();
        if (isFinished)
        {
            GameFinished(this, Game.GetBestOf());
        }
        return isFinished;
    }

    /// <summary>
    /// Loads a saved game using the load manager.
    /// </summary>
    /// <returns>The loaded <see cref="Game"/> instance.</returns>
    public void LoadGame(Game game)
    {
        Game = game;
    }

    public List<Game> LoadedGames()
    {
        return SaveManager.GamesToSave();
    }

    public List<Player> LoadedPlayers()
    {
        return SaveManager.PlayerstoSave();
    }
    
    public void SavePlayers(List<Player> PlayersAllreadySaved)
    {
        LoadManager.LoadPlayers(PlayersAllreadySaved);
    }

    public void SaveGames(List<Game> GamesAllreadySaved)
    {
        LoadManager.LoadGames(GamesAllreadySaved);
    }

    public void SaveGamePlayers()
    {
        bool isInclude = false;
        ReadOnlyCollection<Player> Players = Game.GetPlayers();
        List<Player> LoadedPlayers = new(LoadManager.LoadedPlayers());
        foreach (Player player in Players)
        {
            foreach (Player loadedplayer in LoadedPlayers.
                    Where(p => p.Name == player.Name))
            {
                isInclude = true;
            }
            if (!isInclude) SaveManager.SavePlayer(player);
            isInclude=false;
        }
    }

    /// <summary>
    /// Saves the current game using the save manager.
    /// </summary>
    public void SaveGame()
    {
        SaveManager.SaveGame(Game);
    }

    /// <summary>
    /// Gets the current active round.
    /// </summary>
    /// <returns>The current round, or null if none is active.</returns>
    public Round? GetCurrentRound()
    {
        return Game.GetCurrentRound();
    }

    /// <summary>
    /// Gets the player whose turn it currently is.
    /// </summary>
    /// <returns>The current player, or null if none.</returns>
    public Player? GetCurrentPlayer()
    {
        return Game.CurrentPlayer;
    }

    /// <summary>
    /// Gets the list of players participating in the game.
    /// </summary>
    /// <returns>A read-only collection of players.</returns>
    public ReadOnlyCollection<Player> GetPlayers()
    {
        return Game.GetPlayers();
    }

    /// <summary>
    /// Gets the <see cref="BestOf"/> object containing the score and number of games.
    /// </summary>
    /// <returns>The current BestOf instance.</returns>
    public BestOf GetBestOf()
    {
        return Game.GetBestOf();
    }
}
