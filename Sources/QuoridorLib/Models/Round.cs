using System;
using System.Collections.Generic;

namespace QuoridorLib.Models;

public delegate void Progression(int pourcentage);

/// <summary>
/// Manages a single round of the game, handling player turns, pawn movements,
/// and wall placements. Maintains the current player and interacts with the game board.
/// </summary>
public class Round
{
    private Player CurrentPlayer;
    private readonly Board Board;
    private Game? game;

    public Player CurrentPlayerProperty => CurrentPlayer;

    /// <summary>
    /// Initializes a new instance of the Round class with the given player and board.
    /// </summary>
    /// <param name="player">The player who starts the round.</param>
    /// <param name="board">The game board used during the round.</param>
    public Round(Player player, Board board)
    {
        Board = board;
        CurrentPlayer = player;
    }

    /// <summary>
    /// Switches the current player to the specified player.
    /// </summary>
    /// <param name="player">The player to switch to.</param>
    public void SwitchCurrentPlayer(Player player)
    {
        CurrentPlayer = player;
    }

    /// <summary>
    /// Attempts to move the current player's pawn to the specified position.
    /// </summary>
    /// <param name="newX">The X coordinate of the target position</param>
    /// <param name="newY">The Y coordinate of the target position</param>
    /// <returns>True if the pawn was successfully moved, false otherwise</returns>
    public bool MovePawn(int newX, int newY)
    {
        Position position = new Position(newX, newY);
        bool moved = false;

        if (CurrentPlayer == Board.Pawn1.GetPlayer())
        {
            moved = Board.MovePawn(Board.Pawn1, position);
            if (moved && newX == 8 && game != null)
            {
                game.GetBestOf().AddPlayer1Victory();
                Console.WriteLine($"Score mis à jour - Joueur 1: {game.GetBestOf().GetPlayer1Score()}, Joueur 2: {game.GetBestOf().GetPlayer2Score()}");
            }
        }
        else if (CurrentPlayer == Board.Pawn2.GetPlayer())
        {
            moved = Board.MovePawn(Board.Pawn2, position);
            if (moved && newX == 0 && game != null)
            {
                game.GetBestOf().AddPlayer2Victory();
                Console.WriteLine($"Score mis à jour - Joueur 1: {game.GetBestOf().GetPlayer1Score()}, Joueur 2: {game.GetBestOf().GetPlayer2Score()}");
            }
        }

        return moved;
    }

    /// <summary>
    /// Attempts to place a wall at the specified coordinates and orientation.
    /// </summary>
    /// <param name="x">The X coordinate of the wall</param>
    /// <param name="y">The Y coordinate of the wall</param>
    /// <param name="orientation">The orientation of the wall ("vertical" or "horizontal")</param>
    /// <returns>True if the wall was successfully placed, false otherwise</returns>
    public bool PlacingWall(int x, int y, string orientation)
    {
        if (!Board.IsWallONBoard(x, y, orientation))
        {
            return false;
        }

        List<Position> wallPositions = GetWallPositions(x, y, orientation);

        Wall wall1 = new Wall(wallPositions[0], wallPositions[1]);
        Wall wall2 = new Wall(wallPositions[2], wallPositions[3]);
        
        if (!Board.IsCoupleWallPlaceable(wall1, wall2))
        {
            return false;
        }

        return Board.AddCoupleWall(wall1, wall2, orientation);
    }

    /// <summary>
    /// Gets the game board used in this round.
    /// </summary>
    /// <returns>The game board</returns>
    public Board GetBoard()
    {
        return Board;
    }

    /// <summary>
    /// Sets the game instance for this round.
    /// </summary>
    /// <param name="game">The game instance</param>
    public void SetGame(Game game)
    {
        this.game = game;
    }

    /// <summary>
    /// Gets the positions for a wall couple based on the given coordinates and orientation.
    /// </summary>
    /// <param name="x">The X coordinate</param>
    /// <param name="y">The Y coordinate</param>
    /// <param name="orientation">The orientation of the wall</param>
    /// <returns>A list of positions for the wall couple</returns>
    private static List<Position> GetWallPositions(int x, int y, string orientation)
    {
        List<Position> positions = new List<Position>();

        if (orientation == "vertical")
        {
            positions.Add(new Position(x, y));
            positions.Add(new Position(x, y + 1));
            positions.Add(new Position(x + 1, y));
            positions.Add(new Position(x + 1, y + 1));
        }
        else if (orientation == "horizontal")
        {
            positions.Add(new Position(x, y));
            positions.Add(new Position(x + 1, y));
            positions.Add(new Position(x, y + 1));
            positions.Add(new Position(x + 1, y + 1));
        }

        return positions;
    }
}
