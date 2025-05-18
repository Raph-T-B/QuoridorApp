using System.Numerics;

namespace QuoridorLib.Models
{
    /// <summary>
    /// Represents a wall on the game board, defined by two adjacent positions.
    /// A wall is used to obstruct player movement during the game.
    /// </summary>
    public class Wall
    {
        private readonly Position FirstPosition;
        private readonly Position SecondPosition;

        /// <summary>
        /// Initializes a new instance of the Wall class using coordinates.
        /// </summary>
        /// <param name="firstX">The X coordinate of the first position.</param>
        /// <param name="firstY">The Y coordinate of the first position.</param>
        /// <param name="secondX">The X coordinate of the second position.</param>
        /// <param name="secondY">The Y coordinate of the second position.</param>
        public Wall(int firstX, int firstY, int secondX, int secondY)  
        {
            FirstPosition = new (firstX, firstY);
            SecondPosition = new (secondX, secondY);
        }

        /// <summary>
        /// Initializes a new instance of the Wall class using two positions.
        /// </summary>
        /// <param name="firstposition">The first position of the wall.</param>
        /// <param name="secondPosition">The second position of the wall.</param>
        public Wall(Position firstposition, Position secondPosition) 
        {
            FirstPosition = new (firstposition);
            SecondPosition = new (secondPosition);
        }

        /// <summary>
        /// Get the First Wall's position
        /// </summary>
        /// <returns>The wall's First position</returns>
        public Position GetFirstPosition()
        {
            return FirstPosition.GetPosition();
        }

        /// <summary>
        /// Get the Second Wall's position 
        /// </summary>
        /// <returns>The wall's Second position</returns>
        public Position GetSecondPosition()
        {
            return SecondPosition.GetPosition();
        }
        
    }
}
