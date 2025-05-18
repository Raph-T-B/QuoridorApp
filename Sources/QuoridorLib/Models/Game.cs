
using System.Collections.ObjectModel;

namespace QuoridorLib.Models;

/// <summary>
/// Manages the overall game, including players, rounds, and match conditions.
/// </summary>
public class Game
{
    private readonly List<Player> players;
    private Round? currentRound;
    private readonly BestOf bestOf;

    /// <summary>
    /// Initializes a new instance of the Game class.
    /// </summary>
    public Game()
    {
        players = new List<Player>();
        bestOf = new BestOf(3);
        currentRound = null;
    }

    /// <summary>
    /// Adds a player to the game.
    /// </summary>
    /// <param name="player">The player to add.</param>
    /// <exception cref="InvalidOperationException">Thrown when trying to add more than two players.</exception>
    public void AddPlayer(Player player)
    {
        if (players.Count >= 2)
        {
            throw new InvalidOperationException("Cannot add more than 2 players");
        }
        players.Add(player);
    }

    /// <summary>
    /// Starts a new round if exactly two players are present.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the number of players is not equal to 2.</exception>
    public void LaunchRound()
    {
        if (players.Count != 2)
        {
            throw new InvalidOperationException("Two players are required to start a game");
        }

        Board board = new Board();
        board.Init1vs1QuoridorBoard(
            players[0],
            players[1]
        );
        currentRound = new Round(players[0], board);
    }

    /// <summary>
    /// Gets the player whose turn it currently is.
    /// </summary>
    /// <returns>The current player, or null if no round is active.</returns>
    public Player? GetCurrentPlayer()
    {
        if (currentRound == null)
        {
            return null;
        }
        return currentRound.CurrentPlayerProperty;
    }

    /// <summary>
    /// Gets a read-only collection of players in the game.
    /// </summary>
    /// <returns>A read-only list of players.</returns>
    public ReadOnlyCollection<Player> GetPlayers()
    {
        return players.AsReadOnly();
    }

    /// <summary>
    /// Gets the BestOf match condition object.
    /// </summary>
    /// <returns>The BestOf object defining match conditions.</returns>
    public BestOf GetBestOf()
    {
        return bestOf;
    }

    /// <summary>
    /// Determines if the game is over based on the match score.
    /// </summary>
    /// <returns>True if one of the players has reached the majority of wins; otherwise, false.</returns>
    public bool IsGameOver()
    {
        return bestOf.GetPlayer1Score() >= bestOf.GetNumberOfGames() / 2 + 1 ||
               bestOf.GetPlayer2Score() >= bestOf.GetNumberOfGames() / 2 + 1;
    }

    /// <summary>
    /// Gets the current active round.
    /// </summary>
    /// <returns>The current Round object, or null if no round is active.</returns>
    public Round? GetCurrentRound()
    {
        return currentRound;
    }
}
