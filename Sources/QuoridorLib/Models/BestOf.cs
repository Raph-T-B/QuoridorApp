

namespace QuoridorLib.Models;

/// <summary>
/// Represents a "best of N" match tracker between two players, 
/// keeping track of victories for each player and the total number of games.
/// </summary>
public class BestOf
{
    private int player1Score;
    private int player2Score;
    private readonly int numberOfGames;

    /// <summary>
    /// Initializes a new instance of the BestOf class with a specified number of games.
    /// </summary>
    /// <param name="numberOfGames">Total number of games in the series.</param>
    public BestOf(int numberOfGames)
    {
        this.numberOfGames = numberOfGames;
        player1Score = 0;
        player2Score = 0;
    }

    /// <summary>
    /// Gets the current score (number of victories) for Player 1.
    /// </summary>
    /// <returns>Player 1's score.</returns>
    public int GetPlayer1Score()
    {
        return player1Score;
    }

    /// <summary>
    /// Gets the current score (number of victories) for Player 2.
    /// </summary>
    /// <returns>Player 2's score.</returns>
    public int GetPlayer2Score()
    {
        return player2Score;
    }

    /// <summary>
    /// Adds one victory to Player 1's score.
    /// </summary>
    public void AddPlayer1Victory()
    {
        player1Score++;
    }

    /// <summary>
    /// Adds one victory to Player 2's score.
    /// </summary>
    public void AddPlayer2Victory()
    {
        player2Score++;
    }

    /// <summary>
    /// Gets the total number of games in the "best of" series.
    /// </summary>
    /// <returns>The number of games.</returns>
    public int GetNumberOfGames()
    {
        return numberOfGames;
    }
}
