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

        [Test]
        public void GetPosition_ReturnsFirstPosition()
        {
            // Arrange
            Position firstPos = new(1, 2);
            Position secondPos = new(1, 3);
            Wall wall = new(firstPos, secondPos);

            // Act
            Position result = wall.GetPosition();

            // Assert
            Assert.That(result.GetPositionX(), Is.EqualTo(firstPos.GetPositionX()));
            Assert.That(result.GetPositionY(), Is.EqualTo(firstPos.GetPositionY()));
        }

        [Test]
        public void IsHorizontal_WithHorizontalWall_ReturnsTrue()
        {
            // Arrange
            Position firstPos = new(1, 2);
            Position secondPos = new(2, 2);
            Wall wall = new(firstPos, secondPos);

            // Act
            bool result = wall.IsHorizontal();

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsHorizontal_WithVerticalWall_ReturnsFalse()
        {
            // Arrange
            Position firstPos = new(1, 2);
            Position secondPos = new(1, 3);
            Wall wall = new(firstPos, secondPos);

            // Act
            bool result = wall.IsHorizontal();

            // Assert
            Assert.That(result, Is.False);
        }
    }
}