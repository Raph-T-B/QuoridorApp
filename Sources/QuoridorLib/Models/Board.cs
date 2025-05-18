
using System.Collections.ObjectModel;

namespace QuoridorLib.Models;

/// <summary>
/// Represents the game board, including pawns, walls, and game logic for moves and placements.
/// </summary>
public class Board
{
    public event BoardChangedDelegate? BoardChanged;
    public delegate void BoardChangedDelegate(Board board);

    // Dictionary linking players to their pawns
    private readonly Dictionary<Player, Pawn> Pawns = new();

    public Pawn Pawn1 { get; private set; } = new(new Position(0, 0));
    public Pawn Pawn2 { get; private set; } = new(new Position(0, 0));

    /// <summary>
    /// Gets the collection of wall couples placed on the board.
    /// </summary>
    public IEnumerable<WallCouple> WallCouples => new ReadOnlyCollection<WallCouple>(_wallCouples);

    private readonly List<WallCouple> _wallCouples = new();

    private int BoardWith { get; set; }
    private int BoardHeight { get; set; }

    /// <summary>
    /// Initializes the board for a 1 vs 1 Quoridor game with default pawn positions.
    /// </summary>
    /// <param name="player1">Player 1</param>
    /// <param name="player2">Player 2</param>
    public void Init1vs1QuoridorBoard(Player player1, Player player2)
    {
        Position positionP1 = new(0, 5);
        Position positionP2 = new(8, 5);

        Pawn pawnP1 = new(positionP1);
        Pawn pawnP2 = new(positionP2);

        pawnP1.SetPlayer(player1);
        pawnP2.SetPlayer(player2);

        Pawns.Add(player1, pawnP1);
        Pawns.Add(player2, pawnP2);

        Pawn1 = pawnP1;
        Pawn2 = pawnP2;

        BoardHeight = 9;
        BoardWith = 9;
    }

    /// <summary>
    /// Attempts to add a couple of walls on the board if valid.
    /// </summary>
    /// <param name="wall1">First wall</param>
    /// <param name="wall2">Second wall</param>
    /// <param name="orientation">Orientation of the walls</param>
    /// <returns>True if walls were successfully placed, otherwise false.</returns>
    public bool AddCoupleWall(Wall wall1, Wall wall2, string orientation)
    {
        if (!IsWallONBoard(wall1.GetFirstPosition().GetPositionX(),
                          wall1.GetFirstPosition().GetPositionY(),
                          orientation))
        {
            return false;
        }

        if (!IsCoupleWallPlaceable(wall1, wall2))
        {
            return false;
        }

        _wallCouples.Add(new WallCouple(wall1, wall2, orientation));
        BoardChanged?.Invoke(this);
        return true;
    }

    /// <summary>
    /// Attempts to move a pawn to a specified position following game rules.
    /// </summary>
    /// <param name="pawn">Pawn to move</param>
    /// <param name="position">Target position</param>
    /// <returns>True if the pawn moved, false otherwise.</returns>
    public bool MovePawn(Pawn pawn, Position position)
    {
        if (IsPawnOnBoard(position) &&
            IsCaseBeside(pawn, position) &&
            !IsOnAPawnCase(position) &&
            !IsWallbetween(pawn, position))
        {
            pawn.Move(position);
            BoardChanged?.Invoke(this);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if a wall is between the pawn and the target position.
    /// </summary>
    /// <param name="pawn">Pawn to check</param>
    /// <param name="theCase">Target position</param>
    /// <returns>True if a wall is between, false otherwise.</returns>
    private bool IsWallbetween(Pawn pawn, Position theCase)
    {
        if (WallCouples == null) return false;

        foreach (WallCouple couple in WallCouples)
        {
            List<Wall> theCouple = new() { couple.GetWall1(), couple.GetWall2() };

            foreach (Wall wall in theCouple)
            {
                Position wallFirstP = wall.GetFirstPosition();
                Position wallSecondP = wall.GetSecondPosition();
                Position pawnPosition = pawn.GetPosition();

                if ((Equals(wallFirstP, theCase) && Equals(wallSecondP, pawnPosition)) ||
                    (Equals(wallFirstP, pawnPosition) && Equals(wallSecondP, theCase)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if any pawn occupies the specified position.
    /// </summary>
    /// <param name="theCase">Position to check</param>
    /// <returns>True if a pawn is there, false otherwise.</returns>
    private bool IsOnAPawnCase(Position theCase)
    {
        return Equals(Pawn1.GetPawnPosition(), theCase) ||
               Equals(Pawn2.GetPawnPosition(), theCase);
    }

    /// <summary>
    /// Checks if a position is adjacent to a pawn's current position.
    /// </summary>
    /// <param name="pawn">Pawn to check</param>
    /// <param name="theCase">Position to check</param>
    /// <returns>True if adjacent, false otherwise.</returns>
    private static bool IsCaseBeside(Pawn pawn, Position theCase)
    {
        int xPawn = pawn.GetPositionX();
        int yPawn = pawn.GetPositionY();
        int xNew = theCase.GetPositionX();
        int yNew = theCase.GetPositionY();

        if (pawn.GetPosition() == theCase)
            return false;

        if (xPawn == xNew &&
            (yPawn - yNew == 1 || yPawn - yNew == -1))
            return true;

        if (yPawn == yNew &&
            (xPawn - xNew == 1 || xPawn - xNew == -1))
            return true;

        return false;
    }

    /// <summary>
    /// Checks if the position is within the board boundaries.
    /// </summary>
    /// <param name="position">Position to check</param>
    /// <returns>True if on board, false otherwise.</returns>
    private bool IsPawnOnBoard(Position position)
    {
        int x = position.GetPositionX();
        int y = position.GetPositionY();

        return x >= 0 && x <= BoardWith && y >= 0 && y <= BoardHeight;
    }

    /// <summary>
    /// Validates if a wall position with given orientation fits within board limits.
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <param name="orientation">Wall orientation (vertical/horizontal)</param>
    /// <returns>True if wall fits on board, false otherwise.</returns>
    public static bool IsWallONBoard(int x, int y, string orientation)
    {
        if (orientation == "vertical")
        {
            return x >= 0 && x <= 8 && y >= 0 && y <= 7;
        }
        else // horizontal
        {
            return x >= 0 && x <= 7 && y >= 0 && y <= 8;
        }
    }

    /// <summary>
    /// Checks if two walls can be placed without overlapping or crossing existing walls.
    /// </summary>
    /// <param name="wall1">First wall</param>
    /// <param name="wall2">Second wall</param>
    /// <returns>True if placement is possible, false otherwise.</returns>
    public bool IsCoupleWallPlaceable(Wall wall1, Wall wall2)
    {
        if (WallCouples == null) return true;

        foreach (WallCouple couple in WallCouples)
        {
            List<Wall> theCouple = new() { couple.GetWall1(), couple.GetWall2() };

            foreach (Wall placedWall in theCouple)
            {
                if (AreWallsOverlapping(wall1, placedWall) ||
                    AreWallsOverlapping(wall2, placedWall) ||
                    AreWallsCrossing(wall1, placedWall) ||
                    AreWallsCrossing(wall2, placedWall))
                    return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Checks if two walls overlap (same orientation and overlapping coordinates).
    /// </summary>
    /// <param name="wallA">First wall</param>
    /// <param name="wallB">Second wall</param>
    /// <returns>True if overlapping, false otherwise.</returns>
    private static bool AreWallsOverlapping(Wall wallA, Wall wallB)
    {
        Position a1 = wallA.GetFirstPosition();
        Position a2 = wallA.GetSecondPosition();
        Position b1 = wallB.GetFirstPosition();
        Position b2 = wallB.GetSecondPosition();

        bool sameOrientation =
            (a1.GetPositionX() == a2.GetPositionX() && b1.GetPositionX() == b2.GetPositionX()) ||
            (a1.GetPositionY() == a2.GetPositionY() && b1.GetPositionY() == b2.GetPositionY());

        if (!sameOrientation) return false;

        if (a1.GetPositionX() == a2.GetPositionX())
        {
            return a1.GetPositionX() == b1.GetPositionX() &&
                   ((a1.GetPositionY() <= b1.GetPositionY() && a2.GetPositionY() >= b1.GetPositionY()) ||
                    (b1.GetPositionY() <= a1.GetPositionY() && b2.GetPositionY() >= a1.GetPositionY()));
        }
        else
        {
            return a1.GetPositionY() == b1.GetPositionY() &&
                   ((a1.GetPositionX() <= b1.GetPositionX() && a2.GetPositionX() >= b1.GetPositionX()) ||
                    (b1.GetPositionX() <= a1.GetPositionX() && b2.GetPositionX() >= a1.GetPositionX()));
        }
    }

    /// <summary>
    /// Checks if two walls cross each other perpendicularly.
    /// </summary>
    /// <param name="wallA">First wall</param>
    /// <param name="wallB">Second wall</param>
    /// <returns>True if walls cross, false otherwise.</returns>
    public static bool AreWallsCrossing(Wall wallA, Wall wallB)
    {
        Position a1 = wallA.GetFirstPosition();
        Position a2 = wallA.GetSecondPosition();
        Position b1 = wallB.GetFirstPosition();
        Position b2 = wallB.GetSecondPosition();

        bool isPerpendicular =
            (a1.GetPositionX() == a2.GetPositionX() && b1.GetPositionY() == b2.GetPositionY()) ||
            (a1.GetPositionY() == a2.GetPositionY() && b1.GetPositionX() == b2.GetPositionX());

        if (!isPerpendicular) return false;

        if (a1.GetPositionX() == a2.GetPositionX())
        {
            return a1.GetPositionX() >= Math.Min(b1.GetPositionX(), b2.GetPositionX()) &&
                   a1.GetPositionX() <= Math.Max(b1.GetPositionX(), b2.GetPositionX()) &&
                   b1.GetPositionY() >= Math.Min(a1.GetPositionY(), a2.GetPositionY()) &&
                   b1.GetPositionY() <= Math.Max(a1.GetPositionY(), a2.GetPositionY());
        }
        else
        {
            return a1.GetPositionY() >= Math.Min(b1.GetPositionY(), b2.GetPositionY()) &&
                   a1.GetPositionY() <= Math.Max(b1.GetPositionY(), b2.GetPositionY()) &&
                   b1.GetPositionX() >= Math.Min(a1.GetPositionX(), a2.GetPositionX()) &&
                   b1.GetPositionX() <= Math.Max(a1.GetPositionX(), a2.GetPositionX());
        }
    }
}
