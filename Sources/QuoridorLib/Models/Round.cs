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
    // pour la dependenci
    public delegate void Progression(int pourcentage);
    
   class Round : Board
    {


        public Player CurrentPlayer;
        readonly Board Board;
        public Round(Player player,Board board)
        {
            Board = board ;
            CurrentPlayer = player; 
        }

        public void SwitchCurrentPlayer(Player player)
        {
            CurrentPlayer = player;
        }

        public void MovePawn(int newX, int newY)
        {
            Position position = new Position(newX, newY);
            Board.MovePawn(CurrentPlayer.Name, position);
        }

        private static List<Position> GetWallPositions(int x, int y, string orientation)
        {
            int x1 =x , y1 =y , x2 , y2 , x3 , y3 , x4 = x+1 , y4 = y+1;
            if (orientation == "vertical")
            {
                x2 = x; y2 = y + 1;
                x3 = x + 1; y3 = y;
            }
            else
            {
                x2 = x + 1; y2 = y;
                x3 = x; y3 = y ;
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
            if (Board.IsWallONBoard(x, y, orientation)) {
                return false;
            }

            List<Position> wallspositions = GetWallPositions(x, y, orientation);

            Position position1Wall1 = new(wallspositions[0]);
            Position position2Wall1 = new(wallspositions[1]);
            Position position1Wall2 = new(wallspositions[2]);
            Position position2Wall2 = new(wallspositions[3]);

            Wall wall1 = new Wall(position1Wall1, position2Wall1);
            Wall wall2 = new Wall(position1Wall2, position2Wall2);
            
            Board.AddCoupleWall(wall1, wall2);

            return true;
        }
    }

}
