using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using QuoridorLib.Models;

namespace QuoridorLib.Models
{
    class Pawn : Position
    {

        /// <summary>
        /// Pawn constructor which include Position constructor
        /// </summary>
        /// <param name="x">Position x to set</param>
        /// <param name="y">Position y to set</param>
        public Pawn(int x, int y)
            : base(x,y)
        {
        }
        /// <summary>
        /// Pawn constructor which include Position constructor
        /// </summary>
        /// <param name="position">Position to set</param>
        public Pawn(Position position)
            : base(position)
        {
        }
        /// <summary>
        /// Move the Pawn to another position x & y
        /// </summary>
        /// <param name="x">The new position x to set</param>
        /// <param name="y">The new position y to set</param>
        public void Move(Position position) {
            SetPosition(position.GetPositionX(), position.GetPositionY());
        }
        /// <summary>
        /// Fonction to get the pawn position
        /// </summary>
        /// <returns>The pawn position</returns>
        public Position GetPawnPosition() {
            return GetPosition();
        }
        
    }
}


