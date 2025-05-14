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
    
    public class Round(Player player, Board board)
    {
        public Player CurrentPlayer { get; set; } = player;
        private List<Player> Players = [];
        private readonly Board Board = board;

        public void SwitchCurrentPlayer(Player player)
        {
            CurrentPlayer = player;
        }

        public void AddPlayers(Player player1,Player player2)
        {
            Players.Add(player1);
            Players.Add(player2);
        }

        public void MovePawn(int newX, int newY)
        {
            if (CurrentPlayer == null)
            {
                throw new InvalidOperationException("No current player in the round");
            }
            Position position = new(newX, newY);
            if (CurrentPlayer == Players[0])
                Board.MovePawn(Board.Pawn1, position);
            if (CurrentPlayer == Players[1])
                Board.MovePawn(Board.Pawn2, position);

        }

        private static List<Position> GetWallPositions(int x, int y, string orientation)
        {
            int x1 = x, y1 = y, x2, y2, x3, y3, x4 = x + 1, y4 = y + 1;
            if (orientation == "vertical")
            {
                x2 = x; y2 = y + 1;
                x3 = x + 1; y3 = y;
            }
            else
            {
                x2 = x + 1; y2 = y;
                x3 = x; y3 = y;
            }
            Position position1 = new(x1, y1);
            Position position2 = new(x2, y2);
            Position position3 = new(x3, y3);
            Position position4 = new(x4, y4);
            List<Position> wallPositions = [position1, position2, position3, position4];
            return wallPositions;
        }

        public bool PlacingWall(int x, int y, string orientation)
        {
            if (CurrentPlayer == null)
            {
                throw new InvalidOperationException("No current player in the round");
            }
            
            List<Position> wallspositions = GetWallPositions(x, y, orientation);

            Position position1Wall1 = new(wallspositions[0]);
            Position position2Wall1 = new(wallspositions[1]);
            Position position1Wall2 = new(wallspositions[2]);
            Position position2Wall2 = new(wallspositions[3]);

            Wall wall1 = new(position1Wall1, position2Wall1);
            Wall wall2 = new(position1Wall2, position2Wall2);
            
            return Board.AddCoupleWall(wall1, wall2,orientation);
        }
    }
}
