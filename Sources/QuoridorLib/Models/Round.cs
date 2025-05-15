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
    
    public class Round
    {
        private Player CurrentPlayer;
        private readonly Board Board;
        private Game? game;

        public Player CurrentPlayerProperty => CurrentPlayer;

        public Round(Player player, Board board)
        {
            Board = board;
            CurrentPlayer = player;
        }
        public void SwitchCurrentPlayer(Player player)
        {
            CurrentPlayer = player;
        }

        public bool MovePawn(int newX, int newY)
        {
            Position position = new(newX, newY);
            bool moved;

            if (CurrentPlayer == Board.Pawn1.GetPlayer())
            {
                moved = Board.MovePawn(Board.Pawn1, position);
            }
            else
            {
                moved = Board.MovePawn(Board.Pawn2, position);
            }

            return moved;
        }

        public bool IsSomeoneWin()
        {
            bool victory=false;
            if ( Board.Pawn1.GetPositionX()==8)
            {
                game?.GetBestOf().AddPlayer2Victory();
                victory = true;
            }
            if (Board.Pawn2.GetPositionX() == 0)
            {
                game?.GetBestOf().AddPlayer2Victory();
                victory = true;
            }
            return victory;
        }

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

        public Board GetBoard()
        {
            return Board;
        }

        public void SetGame(Game game)
        {
            this.game = game;
        }
    }
} 