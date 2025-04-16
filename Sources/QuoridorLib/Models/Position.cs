using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorLib.Models
{
    public class Position 
    {
        int X = 0;
        int Y = 0;
        /// <summary>
        /// Position's constructor
        /// </summary>
        /// <param name="x">Position X to set</param>
        /// <param name="y">Position Y to set</param>
        public Position(int x, int y) 
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Position's constructor
        /// </summary>
        /// <param name="position">Position to set</param>
        public Position(Position position)
        {
            X = position.GetPositionX();
            Y = position.GetPositionY();
        }
        /// <summary>
        /// Set the position with nex position x and y
        /// </summary>
        /// <param name="x">The new position X to set</param>
        /// <param name="y">The new position Y to set</param>
        public void SetPosition(int x, int y) 
        {
            X= x;
            Y= y;
        }


        /// <summary>
        /// Get the position
        /// </summary>
        /// <returns>Position itself</returns>
        public Position GetPosition()
        {
            return this;
        }
        /// <summary>
        /// Get the X position
        /// </summary>
        /// <returns>Position X</returns>
        public int GetPositionX()
        {
            return X;
        }
        /// <summary>
        /// Get the Y position
        /// </summary>
        /// <returns>Position Y</returns>
        public int GetPositionY()
        {
            return Y;
        }
        public static bool operator ==(Position leftP, Position rightP)
        {
            if (ReferenceEquals(leftP,rightP)) return true;
            if ( leftP is null || rightP is null) return false;
            return leftP.X == rightP.X && leftP.Y == rightP.Y;
        }

        public static bool operator !=(Position leftP, Position rightP)
        {
            return !(leftP == rightP);
        }
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
