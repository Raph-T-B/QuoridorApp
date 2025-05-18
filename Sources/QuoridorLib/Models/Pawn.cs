using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using QuoridorLib.Models;

namespace QuoridorLib.Models
{
    public class Pawn
    {
        private Position position;
        private Player? player;

        /// <summary>
        /// Pawn constructor which include Position constructor
        /// </summary>
        /// <param name="x">Position x to set</param>
        /// <param name="y">Position y to set</param>
        public Pawn(int x, int y)
        {
            position = new Position(x, y);
        }

        /// <summary>
        /// Pawn constructor which include Position constructor
        /// </summary>
        /// <param name="position">Position to set</param>
        public Pawn(Position position)
        {
            this.position = position;
        }

        /// <summary>
        /// Move the Pawn to another position x & y
        /// </summary>
        /// <param name="x">The new position x to set</param>
        /// <param name="y">The new position y to set</param>
        public void Move(Position newPosition)
        {
            position = newPosition;
        }

        /// <summary>
        /// Fonction to get the pawn position
        /// </summary>
        /// <returns>The pawn position</returns>
        public Position GetPawnPosition()
        {
            return position;
        }
        
        public int GetPositionX()
        {
            return position.GetPositionX();
        }

        public int GetPositionY()
        {
            return position.GetPositionY();
        }

        public Position GetPosition()
        {
            return position;
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        public Player? GetPlayer()
        {
            return player;
        }
    }
}