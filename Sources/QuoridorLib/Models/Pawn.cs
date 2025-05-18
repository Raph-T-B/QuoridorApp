

namespace QuoridorLib.Models;

/// <summary>
/// Represents a pawn in the game, holding its position and associated player.
/// </summary>
public class Pawn
{
    private Position position;
    private Player? player;

    /// <summary>
    /// Initializes a new instance of the Pawn class with specified coordinates.
    /// </summary>
    /// <param name="x">The X coordinate to set for the pawn's position.</param>
    /// <param name="y">The Y coordinate to set for the pawn's position.</param>
    public Pawn(int x, int y)
    {
        position = new Position(x, y);
    }

    /// <summary>
    /// Initializes a new instance of the Pawn class with a specified position.
    /// </summary>
    /// <param name="position">The position to set for the pawn.</param>
    public Pawn(Position position)
    {
        this.position = position;
    }

    /// <summary>
    /// Moves the pawn to a new position.
    /// </summary>
    /// <param name="newPosition">The new position to move the pawn to.</param>
    public void Move(Position newPosition)
    {
        position = newPosition;
    }

    /// <summary>
    /// Gets the current position of the pawn.
    /// </summary>
    /// <returns>The position of the pawn.</returns>
    public Position GetPawnPosition()
    {
        return position;
    }

    /// <summary>
    /// Gets the X coordinate of the pawn's position.
    /// </summary>
    /// <returns>The X coordinate.</returns>
    public int GetPositionX()
    {
        return position.GetPositionX();
    }

    /// <summary>
    /// Gets the Y coordinate of the pawn's position.
    /// </summary>
    /// <returns>The Y coordinate.</returns>
    public int GetPositionY()
    {
        return position.GetPositionY();
    }

    /// <summary>
    /// Gets the current position object of the pawn.
    /// </summary>
    /// <returns>The position of the pawn.</returns>
    public Position GetPosition()
    {
        return position;
    }

    /// <summary>
    /// Sets the player associated with this pawn.
    /// </summary>
    /// <param name="player">The player to associate with the pawn.</param>
    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    /// <summary>
    /// Gets the player associated with this pawn.
    /// </summary>
    /// <returns>The player associated with the pawn, or null if none.</returns>
    public Player? GetPlayer()
    {
        return player;
    }
}
