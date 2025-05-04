using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuoridorLib.Models
{
    
   class Round : Board
    {

        // ✅ Propriété publique avec accès en lecture (get) mais écriture interne uniquement (private set)
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

        public void PlacingWall(int x, int y,string orientation)
        {
            if (orientation == "right")
            {

            }
            Position position1Wall1 = new Position(x1, y1);
            Position position2Wall1 = new Position(x2, y2);
            Position position1Wall2 = new Position(x3 y3);
            Position position2Wall2 = new Position(x4, y4);
            Wall wall1 = new Wall(position1Wall1, position2Wall1);
            Wall wall2 = new Wall(position1Wall2, position2Wall2);
            Board.AddCoupleWall(wall1,wall2);
        }
    }

}
