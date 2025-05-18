using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuoridorLib.Models
{
    public delegate void Progression(int pourcentage);

    /// <summary>
    /// Manages a single round of the game, handling player turns, pawn movements,
    /// and wall placements. Maintains the current player and interacts with the game board.
    /// </summary>
    public class Round
    {
        private Player CurrentPlayer;
        private readonly Board Board;

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
        /// Moves the current player's pawn to the specified coordinates.
        /// </summary>
        /// <param name="newX">The new X coordinate.</param>
        /// <param name="newY">The new Y coordinate.</param
        public void MovePawn(int newX, int newY)
        {
            Position position = new Position(newX, newY);
            if (CurrentPlayer == Board.Pawn1.GetPlayer())
            {
                Board.MovePawn(Board.Pawn1, position);
            }
            else
            {
                Board.MovePawn(Board.Pawn2, position);
            }
        }

        /// <summary>
        /// Attempts to place a wall at the specified coordinates and orientation.
        /// </summary>
        /// <param name="x">The X coordinate of the wall.</param>
        /// <param name="y">The Y coordinate of the wall.</param>
        /// <param name="orientation">The orientation of the wall ("vertical" or "horizontal").</param>
        /// <returns>True if the wall was successfully placed; otherwise, false.</returns>
        public bool PlacingWall(int x, int y, string orientation)
        {
            if (!Board.IsWallONBoard(x, y, orientation))
            {
                return false;
            }

            List<Position> wallspositions = GetWallPositions(x, y, orientation);

            Position position1Wall1 = new(wallspositions[0]);
            Position position2Wall1 = new(wallspositions[1]);
            Position position1Wall2 = new(wallspositions[2]);
            Position position2Wall2 = new(wallspositions[3]);

            Wall wall1 = new Wall(position1Wall1, position2Wall1);
            Wall wall2 = new Wall(position1Wall2, position2Wall2);
            
            return Board.AddCoupleWall(wall1, wall2, orientation);
        }

        /// <summary>
        /// Calculates the four positions needed to place two connected wall segments based on the starting coordinates and orientation.
        /// </summary>
        /// <param name="x">The starting X coordinate of the wall.</param>
        /// <param name="y">The starting Y coordinate of the wall.</param>
        /// <param name="orientation">The orientation of the wall ("vertical" or "horizontal").</param>
        /// <returns>A list of four Position objects representing the wall segments.</returns>
        private static List<Position> GetWallPositions(int x, int y, string orientation)
        {
            int x1 = x, y1 = y, x2, y2, x3, y3, x4, y4;
            if (orientation == "vertical")
            {
                x2 = x; y2 = y + 1;
                x3 = x + 1; y3 = y;
                x4 = x + 1; y4 = y + 1;
            }
            else
            {
                x2 = x + 1; y2 = y;
                x3 = x; y3 = y + 1;
                x4 = x + 1; y4 = y + 1;
            }
            Position position1 = new(x1, y1);
            Position position2 = new(x2, y2);
            Position position3 = new(x3, y3);
            Position position4 = new(x4, y4);
            List<Position> wallPositions = [position1, position2, position3, position4];
            return wallPositions;
        }
    }
} 