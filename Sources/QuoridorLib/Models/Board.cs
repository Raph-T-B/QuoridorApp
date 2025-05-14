using System.Collections.ObjectModel;

namespace QuoridorLib.Models;

public class Board
{
    public event BoardChangedDelegate? BoardChanged;
    public delegate void BoardChangedDelegate(Board board);
    //Dictionary set for a potential Game Update
    private readonly Dictionary<Player, Pawn> Pawns = [];
    public Pawn Pawn1 = new( new Position(0,0) );
    public Pawn Pawn2 = new( new Position(0,0) );
    public IEnumerable<WallCouple> WallCouples
    {
        get => new ReadOnlyCollection<WallCouple>(_wallCouples);
    }
    private readonly List<WallCouple> _wallCouples = [];
    private int BoardWith { get; set; }
    private int BoardHeight { get; set; }

    /// <summary>
    /// Init the Board as a 1vs1 quoridor Board
    /// </summary>
    /// <param name="player1">Player 1's Name</param>
    /// <param name="player2">Player 2's Name</param>
    /// <param name="positionP1">Player 1's Position</param>
    /// <param name="positionP2">Player 2's Position</param>
    public void Init1vs1QuoridorBoard(Player player1, Player player2)
    {
        Position positionP1 = new(0, 5);
        Position positionP2 = new(8, 5);

        Pawn pawnP1 = new(positionP1);
        Pawn pawnP2 = new(positionP2);

        
        Pawns.Add(player1, pawnP1);
        Pawns.Add(player2, pawnP2);

        Pawn1 = new(pawnP1);
        Pawn2 = new(pawnP2);

        BoardHeight = 9;
        BoardWith = 9;
    }


    public bool AddCoupleWall(Wall wall1, Wall wall2, string orientation)
    {
        if (IsWallONBoard(wall1.GetFirstPosition().GetPositionX(),
                          wall1.GetFirstPosition().GetPositionY(),
                          orientation))
            if (IsCoupleWallPlaceable(wall1,wall2)) {
                _wallCouples.Add(new WallCouple(wall1, wall2, orientation));
                return true;
            }
        return false;
    }

    /// <summary>
    /// Move a Pawn if it's possible -> check if the next position: 
    /// - is on board
    /// - is on a beside case of the pawn case
    /// - is not on another pawn case
    /// - is not a position behind a wall
    /// </summary>
    /// <param name="pawnName">The pawn Name</param>
    /// <param name="position">The position where the pawn will go</param>
    /// <returns>True if the Pawn moved, false if not</returns>
    public bool MovePawn(Pawn pawn,Position position)
    {
        if (IsPawnOnBoard(position) &&
            IsCaseBeside(pawn,position) &&
            !IsOnAPawnCase(position) &&
            !IsWallbetween(pawn,position) )
        {
            pawn.Move(position);
            BoardChanged?.Invoke(this);

            return true;
        }
        return false;
    }

    /// <summary>
    /// Check if a Wall is between the Pawn's position and the Position theCase
    /// </summary>
    /// <param name="pawn">The pawn to check</param>
    /// <param name="theCase">The Position to check</param>
    /// <returns>True if a Wall is between, false if not</returns>
    private bool IsWallbetween(Pawn pawn, Position theCase)
    {
        if (WallCouples == null) return false;

        foreach (WallCouple couple in WallCouples)
        {     
            List<Wall> theCouple = [couple.GetWall1(), couple.GetWall2()];

            foreach (Wall wall in theCouple) 
            {
                Position wallFirstP = wall.GetFirstPosition();
                Position wallSecondP = wall.GetSecondPosition();
                Position pawnPosition = pawn.GetPosition();

                if ((wallFirstP == theCase && wallSecondP == pawnPosition) ||
                    (wallFirstP == pawnPosition && wallSecondP == theCase))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Check if a Pawn is on the Position position
    /// </summary>
    /// <param name="theCase">The position to check</param>
    /// <returns>True if a Pawn is on the case, false if not </returns>
    private bool IsOnAPawnCase(Position theCase) 
    {
        if (Equals(Pawn1.GetPawnPosition() , theCase)
            || Equals(Pawn2.GetPawnPosition() , theCase)) 
            return true;
        return false;
    }

    /// <summary>
    /// Check if the position is on a beside case of the pawn position
    /// </summary>
    /// <param name="pawn">The pawn itself</param>
    /// <param name="theCase">The case to check</param>
    /// <returns>True if the case is beside, false if not</returns>
    private static bool IsCaseBeside(Pawn pawn,Position theCase) 
    {
        int xPawn = pawn.GetPositionX();
        int yPawn = pawn.GetPositionY();
        int xNew= theCase.GetPositionX();
        int yNew= theCase.GetPositionY();

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
    /// Check if the position is on the Board
    /// </summary>
    /// <param name="position">Position to check</param>
    /// <returns>True if position in the Board, false if not</returns>
    private bool IsPawnOnBoard(Position position)
    {
        int x = position.GetPositionX();
        int y = position.GetPositionY();

        return x <= BoardWith && x >= 0 && y <= BoardHeight && y >= 0;
    }

    /// <summary>
    /// Check if the given Position is correct to place the wall
    /// </summary>
    /// <param name="x">The x origin of wall's position</param>
    /// <param name="y">The y origin of wall's position</param>
    /// <param name="orientation">The wall orientation</param>
    /// <returns>True if the position is correct, false if not</returns>
    private static bool IsWallONBoard(int x,int y,string orientation)
    {
        if (orientation == "vertical")
        {
            if ( x >= 0 && x <=8
                && y >= 0 && y <= 7 )
                return true;
        }
        else //horizontal
        {
            if (x >= 0 && x <= 7
                && y >= 0 && y <= 8)
                return true;
        }

        return false;
    }
    private bool IsCoupleWallPlaceable(Wall wall1, Wall wall2)
    {
        if (WallCouples == null) return true;

        foreach (WallCouple couple in WallCouples)
        {
            List<Wall>theCouple= [couple.GetWall1(), couple.GetWall2()];

            foreach (Wall placedWall in theCouple)
            {
                if (AreWallsOverlapping(wall1, placedWall) || AreWallsOverlapping(wall2, placedWall))
                    return false;

                // Interdiction de croiser un mur
                if (AreWallsCrossing(wall1, placedWall) || AreWallsCrossing(wall2, placedWall))
                    return false;
            }
        }

        return true;
    }

    private static bool AreWallsOverlapping(Wall wall1, Wall wall2)
    {
        return (wall1.GetFirstPosition().Equals(wall2.GetFirstPosition()) &&
                wall1.GetSecondPosition().Equals(wall2.GetSecondPosition())) ||
               (wall1.GetFirstPosition().Equals(wall2.GetSecondPosition()) &&
                wall1.GetSecondPosition().Equals(wall2.GetFirstPosition()));
    }

    private static bool AreWallsCrossing(Wall wallA, Wall wallB)
    {
        Position a1 = wallA.GetFirstPosition();
        Position a2 = wallA.GetSecondPosition();
        Position b1 = wallB.GetFirstPosition();
        Position b2 = wallB.GetSecondPosition();
       
        bool isPerpendicular =
            (a1.GetPositionX() == a2.GetPositionX() && b1.GetPositionY() == b2.GetPositionY()) ||
            (a1.GetPositionY() == a2.GetPositionY() && b1.GetPositionX() == b2.GetPositionX());

        if (!isPerpendicular) return false;

        return (a1.Equals(b2) || a2.Equals(b1) || a1.Equals(b1) || a2.Equals(b2));
    }

}
