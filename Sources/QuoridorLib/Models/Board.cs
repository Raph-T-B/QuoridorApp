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
    private Game? game;

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
        // Vérifier si les murs sont sur le plateau
        if (!IsWallONBoard(wall1.GetFirstPosition().GetPositionX(),
                          wall1.GetFirstPosition().GetPositionY(),
                          orientation))
        {
            return false;
        }

        // Vérifier si les murs sont valides (adjacents et correctement orientés)
        if (!AreWallsValid(wall1, wall2, orientation))
        {
            return false;
        }

        // Vérifier si les murs peuvent être placés (pas de chevauchement ou croisement)
        if (!IsCoupleWallPlaceable(wall1, wall2))
        {
            return false;
        }

        _wallCouples.Add(new WallCouple(wall1, wall2, orientation));
        BoardChanged?.Invoke(this);
        return true;
    }

    private static bool AreWallsValid(Wall wall1, Wall wall2, string orientation)
    {
        var pos1 = wall1.GetFirstPosition();
        var pos2 = wall1.GetSecondPosition();
        var pos3 = wall2.GetFirstPosition();
        var pos4 = wall2.GetSecondPosition();

        if (orientation == "horizontal")
        {
            // Pour un mur horizontal, les murs doivent être adjacents horizontalement
            return pos1.GetPositionY() == pos2.GetPositionY() && // Premier mur horizontal
                   pos3.GetPositionY() == pos4.GetPositionY() && // Deuxième mur horizontal
                   pos1.GetPositionY() == pos3.GetPositionY() && // Même ligne
                   Math.Abs(pos2.GetPositionX() - pos3.GetPositionX()) == 1; // Adjacents
        }
        else // vertical
        {
            // Pour un mur vertical, les murs doivent être adjacents verticalement
            return pos1.GetPositionX() == pos2.GetPositionX() && // Premier mur vertical
                   pos3.GetPositionX() == pos4.GetPositionX() && // Deuxième mur vertical
                   pos1.GetPositionX() == pos3.GetPositionX() && // Même colonne
                   Math.Abs(pos2.GetPositionY() - pos3.GetPositionY()) == 1; // Adjacents
        }
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
    public bool MovePawn(Pawn pawn, Position position)
    {
        // Vérifier si la position est sur le plateau
        if (!IsPawnOnBoard(position))
        {
            return false;
        }

        // Vérifier si la position est adjacente
        if (!IsCaseBeside(pawn, position))
        {
            return false;
        }

        // Vérifier si la position est occupée par un autre pion
        if (IsOnAPawnCase(position))
        {
            return false;
        }

        // Vérifier s'il y a un mur entre la position actuelle et la nouvelle position
        if (IsWallbetween(pawn, position))
        {
            return false;
        }

        // Si toutes les vérifications sont passées, déplacer le pion
        pawn.Move(position);
        BoardChanged?.Invoke(this);
        return true;
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

                // Vérifier si le mur est entre le pion et la nouvelle position
                if ((wallFirstP.GetPositionX() == theCase.GetPositionX() && 
                     wallFirstP.GetPositionY() == theCase.GetPositionY() && 
                     wallSecondP.GetPositionX() == pawnPosition.GetPositionX() && 
                     wallSecondP.GetPositionY() == pawnPosition.GetPositionY()) ||
                    (wallFirstP.GetPositionX() == pawnPosition.GetPositionX() && 
                     wallFirstP.GetPositionY() == pawnPosition.GetPositionY() && 
                     wallSecondP.GetPositionX() == theCase.GetPositionX() && 
                     wallSecondP.GetPositionY() == theCase.GetPositionY()))
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
        return (Pawn1.GetPawnPosition().GetPositionX() == theCase.GetPositionX() && 
                Pawn1.GetPawnPosition().GetPositionY() == theCase.GetPositionY()) ||
               (Pawn2.GetPawnPosition().GetPositionX() == theCase.GetPositionX() && 
                Pawn2.GetPawnPosition().GetPositionY() == theCase.GetPositionY());
    }

    /// <summary>
    /// Check if the position is on a beside case of the pawn position
    /// </summary>
    /// <param name="pawn">The pawn itself</param>
    /// <param name="theCase">The case to check</param>
    /// <returns>True if the case is beside, false if not</returns>
    private static bool IsCaseBeside(Pawn pawn, Position theCase)
    {
        int xPawn = pawn.GetPositionX();
        int yPawn = pawn.GetPositionY();
        int xNew = theCase.GetPositionX();
        int yNew = theCase.GetPositionY();

        // Vérifier si c'est la même position
        if (xPawn == xNew && yPawn == yNew)
            return false;

        // Vérifier les mouvements horizontaux et verticaux
        bool isHorizontalMove = yPawn == yNew && Math.Abs(xPawn - xNew) == 1;
        bool isVerticalMove = xPawn == xNew && Math.Abs(yPawn - yNew) == 1;

        return isHorizontalMove || isVerticalMove;
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
        return x >= 0 && x < 9 && y >= 0 && y < 9;
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
        if (orientation == "horizontal")
        {
            // Pour un mur horizontal, x doit être entre 0 et 7, y entre 0 et 8
            return x >= 0 && x <= 7 && y >= 0 && y <= 8;
        }
        else // vertical
        {
            // Pour un mur vertical, x doit être entre 0 et 8, y entre 0 et 7
            return x >= 0 && x <= 8 && y >= 0 && y <= 7;
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
                if (AreWallsOverlapping(wall1, placedWall) || 
                    AreWallsOverlapping(wall2, placedWall) ||
                    AreWallsCrossing(wall1, placedWall) || 
                    AreWallsCrossing(wall2, placedWall))
                    return false;
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

    public Dictionary<Player, Position> GetPawnsPositions()
    {
        Dictionary<Player, Position> positions = [];
        var player1 = Pawn1.GetPlayer();
        var player2 = Pawn2.GetPlayer();
        
        
        if (player1 != null)
        {
            positions.Add(player1, Pawn1.GetPawnPosition());
        }
        if (player2 != null)
        {
            positions.Add(player2, Pawn2.GetPawnPosition());
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

    public void SetGame(Game game)
    {
        this.game = game;
    }

    public bool IsVictoryPosition(Position position, Player player)
    {
        if (player == Pawn1.GetPlayer())
        {
            return position.GetPositionX() == 8;
        }
        else if (player == Pawn2.GetPlayer())
        {
            return position.GetPositionX() == 0;
        }
        return false;
    }
}