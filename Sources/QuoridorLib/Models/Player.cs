namespace QuoridorLib.Models;

/// <summary>
/// Represents a player in the game with a name and a count of victories.
/// </summary>
public class Player
{
    private readonly string name;
    private uint victories;

    /// <summary>
    /// Initializes a new instance of the Player class with the specified name.
    /// </summary>
    /// <param name="name">The name of the player.</param>
    public Player(string name)
    {
        this.name = name;
        this.victories = 0;
    }

    /// <summary>
    /// Gets the name of the player.
    /// </summary>
    public string Name
    {
        get { return name; }
    }

    /// <summary>
    /// Gets the number of victories the player has.
    /// </summary>
    public uint Victories
    {
        get { return victories; }
    }

    /// <summary>
    /// Increments the player's victory count by one.
    /// </summary>
    private void AddVictory()
    {
        victories++;
    }
}