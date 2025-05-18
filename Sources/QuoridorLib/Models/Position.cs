/// <summary>
/// Represents a position on the game board with X and Y coordinates.
/// </summary>
public class Position
{
    /// <summary>
    /// Gets or sets the X coordinate.
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate.
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// Initializes a new instance of the Position class with specified coordinates.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Initializes a new instance of the Position class by copying another position.
    /// </summary>
    /// <param name="position">The position to copy.</param>
    public Position(Position position)
    {
        X = position.X;
        Y = position.Y;
    }

    /// <summary>
    /// Sets the coordinates of the current position.
    /// </summary>
    /// <param name="x">The new X coordinate.</param>
    /// <param name="y">The new Y coordinate.</param>
    public void SetPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Gets the current position instance.
    /// </summary>
    /// <returns>The current Position object.</returns>
    public Position GetPosition() => this;

    /// <summary>
    /// Gets the X coordinate of the position.
    /// </summary>
    /// <returns>The X coordinate.</returns>
    public int GetPositionX() => X;

    /// <summary>
    /// Gets the Y coordinate of the position.
    /// </summary>
    /// <returns>The Y coordinate.</returns>
    public int GetPositionY() => Y;

    /// <summary>
    /// Determines whether the specified object is equal to the current position.
    /// </summary>
    /// <param name="obj">The object to compare with the current position.</param>
    /// <returns>True if the specified object is a Position with the same coordinates; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Position other)
        {
            return X == other.X && Y == other.Y;
        }
        return false;
    }

    /// <summary>
    /// Returns the hash code for this position.
    /// </summary>
    /// <returns>A hash code based on the X and Y coordinates.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
