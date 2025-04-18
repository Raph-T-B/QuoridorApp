using Xunit;
using QuoridorLib.Models;

namespace QuoridorTest.ModelsTest
{
    public class WallTests
    {
        

        [Fact]
        public void GetSecondPosition_Should_Return_Correct_End_Position()
        {
            // Arrange
            Wall wall = new Wall(1, 1, 2, 1);
            Position expectedEndPosition = new Position(2, 1);

            // Act
            Position actualEndPosition = wall.GetSecondPosition();

            // Assert
            Assert.Equal(expectedEndPosition.GetPositionX(), actualEndPosition.GetPositionX());
            Assert.Equal(expectedEndPosition.GetPositionY(), actualEndPosition.GetPositionY());
        }
    }
}