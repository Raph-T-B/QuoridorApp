
namespace QuoridorLib.Models
{
    /// <summary>
    /// Represents a pair of walls placed together with a specific orientation.
    /// </summary>
    public class WallCouple
    {
        readonly Wall Wall1;
        readonly Wall Wall2;
        readonly string Orientation;

        /// <summary>
        /// Initializes a new instance of the WallCouple class.
        /// </summary>
        /// <param name="wall1">The first wall.</param>
        /// <param name="wall2">The second wall.</param>
        /// <param name="orientation">The orientation of the wall couple (e.g., "vertical" or "horizontal").</param>
        public WallCouple(Wall wall1, Wall wall2, string orientation)
        {
            Wall1 = wall1;
            Wall2 = wall2;
            Orientation = orientation;
        }

        /// <summary>
        /// Gets the first wall of the couple.
        /// </summary>
        /// <returns>The first Wall object.</returns>
        public Wall GetWall1()
        {
            return Wall1;
        }

        /// <summary>
        /// Gets the second wall of the couple.
        /// </summary>
        /// <returns>The second Wall object.</returns>
        public Wall GetWall2()
        {
            return Wall2;
        }

        /// <summary>
        /// Gets the orientation of the wall couple.
        /// </summary>
        /// <returns>A string representing the orientation.</returns>
        public string GetOrientation()
        {
            return Orientation;
        }
    }
}
