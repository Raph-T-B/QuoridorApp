using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace QuoridorLib.Models;

/// <summary>
/// Manages the overall game, including players, rounds, and match conditions.
/// </summary>
public class Game
{
    [JsonInclude]
    private readonly List<Player> Players;
    [JsonInclude]
    private Round CurrentRound ;
    [JsonInclude]
    private readonly BestOf BestOf;

    [JsonConstructor]
    public Game(Round currentRound,List<Player> players, BestOf bestof) 
    {
        Players = players;
        CurrentRound = currentRound;
        BestOf = bestof;

    }

    /// <summary>
    /// Initializes a new instance of the Game class.
    /// </summary>
    public Game(int numberOfGames = 1)
    {
        Players = [];
        BestOf = new BestOf(numberOfGames);
        CurrentRound = new(new(""), new());
        CurrentRound.PropertyChanged += Round_PropertyChanged;
    }


    /// <summary>
    /// Adds a player to the game.
    /// </summary>
    /// <param name="player">The player to add.</param>
    /// <exception cref="InvalidOperationException">Thrown when trying to add more than two players.</exception>
    public void AddPlayer(Player player)
    {
        if (Players.Count >= 2)
        {
            throw new InvalidOperationException("Cannot add more than 2 players");
        }
        Players.Add(player);
    }

    /// <summary>
    /// Starts a new round if exactly two players are present.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the number of players is not equal to 2.</exception>
    public void LaunchRound()
    {
        if (Players.Count != 2)
        {
            throw new InvalidOperationException("Two players are required to start a game");
        }

            Board board = new ();
            board.Init1vs1QuoridorBoard(
                Players[0],
                Players[1]
            );
            CurrentRound = new Round(Players[0], board);
        }

    public Player? CurrentPlayer
    {
        get
        {
            if (CurrentRound == null)
            {
                return null;
            }
            return CurrentRound.CurrentPlayerProperty;
        }
    }

    /// <summary>
    /// Gets the player whose turn it currently is.
    /// </summary>
    /// <returns>The current player, or null if no round is active.</returns>
    public Player? GetCurrentPlayer()
    {
        if (CurrentRound == null)
        {
            return null;
        }
        return CurrentRound.CurrentPlayerProperty;
    }

    /// <summary>
    /// Gets a read-only collection of players in the game.
    /// </summary>
    /// <returns>A read-only list of players.</returns>
    public ReadOnlyCollection<Player> GetPlayers()
    {
        return Players.AsReadOnly();
    }

    /// <summary>
    /// Gets the BestOf match condition object.
    /// </summary>
    /// <returns>The BestOf object defining match conditions.</returns>
    public BestOf GetBestOf()
    {
        return BestOf;
    }

    /// <summary>
    /// Determines if the game is over based on the match score.
    /// </summary>
    /// <returns>True if one of the players has reached the majority of wins; otherwise, false.</returns>
    public bool IsGameOver()
    {
        return BestOf.GetPlayer1Score() >= BestOf.GetNumberOfGames() / 2 + 1 ||
               BestOf.GetPlayer2Score() >= BestOf.GetNumberOfGames() / 2 + 1;
    }

    private void Round_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        
        if (Players[0] == CurrentRound.GetBoard().Pawn1.GetPlayer())
        {
            if (CurrentRound.GetBoard().Pawn1.GetPositionX() == 8)
            {
                GetBestOf().AddPlayer1Victory();
            }
        }
        else if (Players[1] == CurrentRound.GetBoard().Pawn2.GetPlayer())
        {
            if (CurrentRound.GetBoard().Pawn2.GetPositionX() == 8)
            {
                GetBestOf().AddPlayer2Victory();
            }
        }
    }
    /// <summary>
    /// Gets the current active round.
    /// </summary>
    /// <returns>The current Round object, or null if no round is active.</returns>
    public Round? GetCurrentRound()
    {
        return CurrentRound;
    }
}
