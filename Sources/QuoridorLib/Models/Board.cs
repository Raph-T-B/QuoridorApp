using System.Collections.ObjectModel;
using QuoridorLib.Observer;

namespace QuoridorLib.Models;

/// <summary>
/// Represents the game board, including pawns, walls, and game logic for moves and placements.
/// </summary>
public class Board : ObservableObject
{
    public event BoardChangedDelegate? BoardChanged;
    public delegate void BoardChangedDelegate(Board board);

    // Dictionary linking players to their pawns
    private readonly Dictionary<Player, Pawn> Pawns = [];

    public Pawn Pawn1 { get; private set; } = new(new Position(0, 0));
    public Pawn Pawn2 { get; private set; } = new(new Position(0, 0));

    /// <summary>
    /// Gets the collection of wall couples placed on the board.
    /// </summary>
    public IEnumerable<WallCouple> WallCouples => new ReadOnlyCollection<WallCouple>(_wallCouples);

    private readonly List<WallCouple> _wallCouples = [];

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

        Position pawnPosition = pawn.GetPosition();
        int pawnX = pawnPosition.GetPositionX();
        int pawnY = pawnPosition.GetPositionY();
        int caseX = theCase.GetPositionX();
        int caseY = theCase.GetPositionY();

        return WallCouples.Any(couple => IsWallBlockingMovement(couple, pawnX, pawnY, caseX, caseY));
    }

    /// <summary>
    /// Checks if a wall couple is blocking the movement between two positions.
    /// </summary>
    /// <param name="couple">The wall couple to check</param>
    /// <param name="pawnX">X coordinate of the pawn's position</param>
    /// <param name="pawnY">Y coordinate of the pawn's position</param>
    /// <param name="caseX">X coordinate of the target position</param>
    /// <param name="caseY">Y coordinate of the target position</param>
    /// <returns>True if the wall couple blocks the movement, false otherwise</returns>
    private static bool IsWallBlockingMovement(WallCouple couple, int pawnX, int pawnY, int caseX, int caseY)
    {
        string orientation = couple.GetOrientation();
        Wall wall1 = couple.GetWall1();
        Wall wall2 = couple.GetWall2();

        if (orientation == "horizontal")
        {
            return IsHorizontalWallBlocking(wall1, wall2, pawnX, pawnY, caseX, caseY);
        }
        else if (orientation == "vertical")
        {
            return IsVerticalWallBlocking(wall1, wall2, pawnX, pawnY, caseX, caseY);
        }
        return false;
    }

    /// <summary>
    /// Checks if a horizontal wall couple is blocking the movement between two positions.
    /// </summary>
    /// <param name="wall1">First wall of the couple</param>
    /// <param name="wall2">Second wall of the couple</param>
    /// <param name="pawnX">X coordinate of the pawn's position</param>
    /// <param name="pawnY">Y coordinate of the pawn's position</param>
    /// <param name="caseX">X coordinate of the target position</param>
    /// <param name="caseY">Y coordinate of the target position</param>
    /// <returns>True if the horizontal wall couple blocks the movement, false otherwise</returns>
    private static bool IsHorizontalWallBlocking(Wall wall1, Wall wall2, int pawnX, int pawnY, int caseX, int caseY)
    {
        if (pawnX != caseX) return false;

        int wallY = wall1.GetFirstPosition().GetPositionY();
        int wallX1 = wall1.GetFirstPosition().GetPositionX();
        int wallX2 = wall2.GetFirstPosition().GetPositionX();

        return wallY == Math.Min(pawnY, caseY) &&
               wallX1 <= Math.Max(pawnX, caseX) &&
               wallX2 >= Math.Min(pawnX, caseX) &&
               Math.Abs(pawnY - caseY) == 1;
    }

    /// <summary>
    /// Checks if a vertical wall couple is blocking the movement between two positions.
    /// </summary>
    /// <param name="wall1">First wall of the couple</param>
    /// <param name="wall2">Second wall of the couple</param>
    /// <param name="pawnX">X coordinate of the pawn's position</param>
    /// <param name="pawnY">Y coordinate of the pawn's position</param>
    /// <param name="caseX">X coordinate of the target position</param>
    /// <param name="caseY">Y coordinate of the target position</param>
    /// <returns>True if the vertical wall couple blocks the movement, false otherwise</returns>
    private static bool IsVerticalWallBlocking(Wall wall1, Wall wall2, int pawnX, int pawnY, int caseX, int caseY)
    {
        if (pawnY != caseY) return false;

        int wallX = wall1.GetSecondPosition().GetPositionX();
        int wallY1 = wall1.GetFirstPosition().GetPositionY();
        int wallY2 = wall2.GetFirstPosition().GetPositionY();

        return wallX == Math.Min(pawnX, caseX) + 1 &&
               wallY1 <= Math.Max(pawnY, caseY) &&
               wallY2 >= Math.Min(pawnY, caseY) &&
               Math.Abs(pawnX - caseX) == 1;
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

        return x < BoardWith && x >= 0 && y < BoardHeight && y >= 0;
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
        else if (orientation == "horizontal") // horizontal
        {
            return x >= 0 && x <= 7 && y >= 0 && y <= 8;
        }
        return false;
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

        return !WallCouples.Any(couple => IsWallCoupleInvalid(wall1, wall2, couple));
    }

    private static bool IsWallCoupleInvalid(Wall wall1, Wall wall2, WallCouple couple)
    {
        return new[] { couple.GetWall1(), couple.GetWall2() }
            .Any(placedWall => IsWallInvalid(wall1, wall2, placedWall));
    }

    private static bool IsWallInvalid(Wall wall1, Wall wall2, Wall placedWall)
    {
        return AreWallsOverlapping(wall1, placedWall) || 
               AreWallsOverlapping(wall2, placedWall) ||
               AreWallsCrossing(wall1, placedWall) || 
               AreWallsCrossing(wall2, placedWall) ||
               AreWallsAdjacent(wall1, placedWall) || 
               AreWallsAdjacent(wall2, placedWall);
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

        if (!HaveSameOrientation(a1, a2, b1, b2)) return false;

        return IsWallOverlapping(a1, a2, b1, b2);
    }

    private static bool HaveSameOrientation(Position a1, Position a2, Position b1, Position b2)
    {
        bool aIsVertical = a1.GetPositionX() == a2.GetPositionX();
        bool bIsVertical = b1.GetPositionX() == b2.GetPositionX();
        return aIsVertical == bIsVertical;
    }

    private static bool IsWallOverlapping(Position a1, Position a2, Position b1, Position b2)
    {
        bool isVertical = a1.GetPositionX() == a2.GetPositionX();
        if (isVertical)
        {
            return AreVerticalWallsOverlapping(a1, a2, b1, b2);
        }
        return AreHorizontalWallsOverlapping(a1, a2, b1, b2);
    }

    private static bool AreVerticalWallsOverlapping(Position a1, Position a2, Position b1, Position b2)
    {
        int aX = a1.GetPositionX();
        int bX = b1.GetPositionX();
        if (Math.Abs(aX - bX) > 1) return false;

        int aMinY = Math.Min(a1.GetPositionY(), a2.GetPositionY());
        int aMaxY = Math.Max(a1.GetPositionY(), a2.GetPositionY());
        int bMinY = Math.Min(b1.GetPositionY(), b2.GetPositionY());
        int bMaxY = Math.Max(b1.GetPositionY(), b2.GetPositionY());

        return aMinY <= bMaxY && bMinY <= aMaxY;
    }

    private static bool AreHorizontalWallsOverlapping(Position a1, Position a2, Position b1, Position b2)
    {
        int aY = a1.GetPositionY();
        int bY = b1.GetPositionY();
        if (Math.Abs(aY - bY) > 1) return false;

        int aMinX = Math.Min(a1.GetPositionX(), a2.GetPositionX());
        int aMaxX = Math.Max(a1.GetPositionX(), a2.GetPositionX());
        int bMinX = Math.Min(b1.GetPositionX(), b2.GetPositionX());
        int bMaxX = Math.Max(b1.GetPositionX(), b2.GetPositionX());

        return aMinX <= bMaxX && bMinX <= aMaxX;
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

    private static bool AreWallsAdjacent(Wall wallA, Wall wallB)
    {
        Position a1 = wallA.GetFirstPosition();
        Position a2 = wallA.GetSecondPosition();
        Position b1 = wallB.GetFirstPosition();
        Position b2 = wallB.GetSecondPosition();

        if (!HaveSameOrientation(a1, a2, b1, b2)) return false;

        return IsWallAdjacent(a1, a2, b1, b2);
    }

    private static bool IsWallAdjacent(Position a1, Position a2, Position b1, Position b2)
    {
        bool isVertical = a1.GetPositionX() == a2.GetPositionX();
        if (isVertical)
        {
            return AreVerticalWallsAdjacent(a1, a2, b1, b2);
        }
        return AreHorizontalWallsAdjacent(a1, a2, b1, b2);
    }

    private static bool AreVerticalWallsAdjacent(Position a1, Position a2, Position b1, Position b2)
    {
        if (Math.Abs(a1.GetPositionX() - b1.GetPositionX()) != 1) return false;

        return (a1.GetPositionY() <= b2.GetPositionY() && a2.GetPositionY() >= b1.GetPositionY()) ||
               (b1.GetPositionY() <= a2.GetPositionY() && b2.GetPositionY() >= a1.GetPositionY());
    }

    private static bool AreHorizontalWallsAdjacent(Position a1, Position a2, Position b1, Position b2)
    {
        if (Math.Abs(a1.GetPositionY() - b1.GetPositionY()) != 1) return false;

        return (a1.GetPositionX() <= b2.GetPositionX() && a2.GetPositionX() >= b1.GetPositionX()) ||
               (b1.GetPositionX() <= a2.GetPositionX() && b2.GetPositionX() >= a1.GetPositionX());
    }

    public Dictionary<Player, Position> GetPawnsPositions()
    {
        return Pawns.ToDictionary(
            pair => pair.Key,
            pair => pair.Value.GetPawnPosition()
        );
    }

    public List<(Position p1, Position p2)> GetWallsPositions()
    {
        return [.. WallCouples.SelectMany(couple => new[]
        {
            (couple.GetWall1().GetFirstPosition(), couple.GetWall1().GetSecondPosition()),
            (couple.GetWall2().GetFirstPosition(), couple.GetWall2().GetSecondPosition())
        })];
    }

    public List<Position> GetPossibleMoves(Pawn pawn)
    {
        Position currentPos = pawn.GetPosition();
        int x = currentPos.GetPositionX();
        int y = currentPos.GetPositionY();

        Position[] directions = [
            new Position(x + 1, y), // droite
            new Position(x - 1, y), // gauche
            new Position(x, y + 1), // bas
            new Position(x, y - 1)  // haut
        ];

        return [.. directions.Where(pos => 
            IsPawnOnBoard(pos) && 
            IsCaseBeside(pawn, pos) && 
            !IsOnAPawnCase(pos) && 
            !IsWallbetween(pawn, pos)
        )];
    }

}