using System.Collections.ObjectModel;

namespace QuoridorLib.Models;

public class Board
{
    public event BoardChangedDelegate? BoardChanged;
    public delegate void BoardChangedDelegate(Board board);
    //Dictionary set for a potential Game Update
    private readonly Dictionary<Player, Pawn> Pawns = [];
    public Pawn Pawn1 { get; private set; } = new(new Position(0, 0));
    public Pawn Pawn2 { get; private set; } = new(new Position(0, 0));
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

        pawnP1.SetPlayer(player1);
        pawnP2.SetPlayer(player2);
        
        Pawns.Add(player1, pawnP1);
        Pawns.Add(player2, pawnP2);

        Pawn1 = pawnP1;
        Pawn2 = pawnP2;

        BoardHeight = 9;
        BoardWith = 9;
    }


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

        Position pawnPosition = pawn.GetPosition();
        int pawnX = pawnPosition.GetPositionX();
        int pawnY = pawnPosition.GetPositionY();
        int caseX = theCase.GetPositionX();
        int caseY = theCase.GetPositionY();

        foreach (WallCouple couple in WallCouples)
        {     
            string orientation = couple.GetOrientation();
            Wall wall1 = couple.GetWall1();
            Wall wall2 = couple.GetWall2();

            // Pour un mur horizontal
            if (orientation == "horizontal")
            {
                // Vérifie si le mur bloque un déplacement vertical
                if (pawnX == caseX && pawnY != caseY)
                {
                    int wallY = wall1.GetFirstPosition().GetPositionY();
                    int wallX1 = wall1.GetFirstPosition().GetPositionX();
                    int wallX2 = wall2.GetFirstPosition().GetPositionX();

                    // Vérifie si le mur est exactement entre les deux positions
                    if (wallY == Math.Min(pawnY, caseY) + 1 &&
                        wallX1 <= Math.Max(pawnX, caseX) &&
                        wallX2 >= Math.Min(pawnX, caseX) &&
                        Math.Abs(pawnY - caseY) == 1) // Vérifie que c'est un mouvement d'une seule case
                    {
                        return true;
                    }
                }
            }
            // Pour un mur vertical
            else if (orientation == "vertical")
            {
                // Vérifie si le mur bloque un déplacement horizontal
                if (pawnY == caseY && pawnX != caseX)
                {
                    int wallX = wall1.GetFirstPosition().GetPositionX();
                    int wallY1 = wall1.GetFirstPosition().GetPositionY();
                    int wallY2 = wall2.GetFirstPosition().GetPositionY();

                    // Vérifie si le mur est exactement entre les deux positions
                    if ((wallX == Math.Min(pawnX, caseX) || wallX == Math.Max(pawnX, caseX)) &&
                        wallY1 <= Math.Max(pawnY, caseY) &&
                        wallY2 >= Math.Min(pawnY, caseY) &&
                        Math.Abs(pawnX - caseX) == 1) // Vérifie que c'est un mouvement d'une seule case
                    {
                        return true;
                    }
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

        return x < BoardWith && x >= 0 && y < BoardHeight && y >= 0;
    }

    /// <summary>
    /// Check if the given Position is correct to place the wall
    /// </summary>
    /// <param name="x">The x origin of wall's position</param>
    /// <param name="y">The y origin of wall's position</param>
    /// <param name="orientation">The wall orientation</param>
    /// <returns>True if the position is correct, false if not</returns>
    public static bool IsWallONBoard(int x, int y, string orientation)
    {
        if (orientation == "vertical")
        {
            return x >= 0 && x <= 8 && y >= 0 && y <= 7;
        }
        else //horizontal
        {
            return x >= 0 && x <= 7 && y >= 0 && y <= 8;
        }
    }
    public bool IsCoupleWallPlaceable(Wall wall1, Wall wall2)
    {
        if (WallCouples == null) return true;

        foreach (WallCouple couple in WallCouples)
        {
            List<Wall> theCouple = [couple.GetWall1(), couple.GetWall2()];

            foreach (Wall placedWall in theCouple)
            {
                // Vérifie si les murs se chevauchent
                if (AreWallsOverlapping(wall1, placedWall) || 
                    AreWallsOverlapping(wall2, placedWall))
                {
                    return false;
                }

                // Vérifie si les murs se croisent
                if (AreWallsCrossing(wall1, placedWall) || 
                    AreWallsCrossing(wall2, placedWall))
                {
                    return false;
                }

                // Vérifie si les murs sont adjacents et de même orientation
                if (AreWallsAdjacent(wall1, placedWall) || 
                    AreWallsAdjacent(wall2, placedWall))
                {
                    return false;
                }
            }
        }

        return true;
    }

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

        // Vérifie si les murs sont de même orientation
        bool sameOrientation = 
            (a1.GetPositionX() == a2.GetPositionX() && b1.GetPositionX() == b2.GetPositionX()) ||
            (a1.GetPositionY() == a2.GetPositionY() && b1.GetPositionY() == b2.GetPositionY());

        if (!sameOrientation) return false;

        // Vérifie si les murs sont adjacents
        if (a1.GetPositionX() == a2.GetPositionX()) // Murs verticaux
        {
            // Vérifie si les murs sont sur des colonnes adjacentes
            if (Math.Abs(a1.GetPositionX() - b1.GetPositionX()) != 1) return false;

            // Vérifie si les murs se chevauchent verticalement
            return (a1.GetPositionY() <= b2.GetPositionY() && a2.GetPositionY() >= b1.GetPositionY()) ||
                   (b1.GetPositionY() <= a2.GetPositionY() && b2.GetPositionY() >= a1.GetPositionY());
        }
        else // Murs horizontaux
        {
            // Vérifie si les murs sont sur des lignes adjacentes
            if (Math.Abs(a1.GetPositionY() - b1.GetPositionY()) != 1) return false;

            // Vérifie si les murs se chevauchent horizontalement
            return (a1.GetPositionX() <= b2.GetPositionX() && a2.GetPositionX() >= b1.GetPositionX()) ||
                   (b1.GetPositionX() <= a2.GetPositionX() && b2.GetPositionX() >= a1.GetPositionX());
        }
    }

    public Dictionary<Player, Position> GetPawnsPositions()
    {
        Dictionary<Player, Position> positions = [];
        foreach (var pair in Pawns)
        {
            positions.Add(pair.Key, pair.Value.GetPawnPosition());
        }
        return positions;
    }

    public List<(Position p1, Position p2)> GetWallsPositions()
    {
        List<(Position p1, Position p2)> positions = [];
        foreach (var couple in WallCouples)
        {
            positions.Add((couple.GetWall1().GetFirstPosition(), couple.GetWall1().GetSecondPosition()));
            positions.Add((couple.GetWall2().GetFirstPosition(), couple.GetWall2().GetSecondPosition()));
        }
        return positions;
    }

    public List<Position> GetPossibleMoves(Pawn pawn)
    {
        List<Position> possibleMoves = [];
        Position currentPos = pawn.GetPosition();
        int x = currentPos.GetPositionX();
        int y = currentPos.GetPositionY();

        // Vérifier les 4 directions possibles
        Position[] directions = [
            new Position(x + 1, y), // droite
            new Position(x - 1, y), // gauche
            new Position(x, y + 1), // bas
            new Position(x, y - 1)  // haut
        ];

        foreach (Position pos in directions)
        {
            if (IsPawnOnBoard(pos) && 
                IsCaseBeside(pawn, pos) && 
                !IsOnAPawnCase(pos) && 
                !IsWallbetween(pawn, pos))
            {
                possibleMoves.Add(pos);
            }
        }

        return possibleMoves;
    }

}