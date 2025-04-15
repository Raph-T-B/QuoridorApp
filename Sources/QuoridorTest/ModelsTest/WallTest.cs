using Xunit;
using QuoridorLib.Models;

namespace QuoridorTest.ModelsTest
{
    public class WallTests
    {
        [Fact]
        public void IsPlaceable_Should_Return_True_If_Placeable_A_Correct_Horizontal_Wall()
        {
            // Arrange
            Wall wall = new Wall(1, 1, 2, 1);

            // Act
            bool result = wall.IsPlaceable();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsPlaceable_Should_Return_True_If_Placeable_A_Correct_Vertical_Wall()
        {
            // Arrange
            Wall wall = new Wall(1, 1, 1, 2);

            // Act
            bool result = wall.IsPlaceable();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsPlaceable_Should_Return_False_If_Placeable_A_Invalid_Diagonal_Wall()
        {
            // Arrange
            Wall wall = new Wall(1, 1, 2, 2);

            // Act
            bool result = wall.IsPlaceable();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsPlaceable_Should_Return_False_If_Placeable_An_Out_Of_Bounds_Wall()
        {
            // Arrange
            Wall wall = new Wall(-1, 1, 0, 1);

            // Act
            bool result = wall.IsPlaceable();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsPlaceable_Should_Return_False_If_Placeable_A_Same_Coordinate_Wall()
        {
            // Arrange
            Wall wall = new Wall(1, 1, 1, 1);

            // Act
            bool result = wall.IsPlaceable();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsPlaceable_Should_Return_False_If_Placeable_A_Horizontal_Wall_That_Is_Too_Long()
        {
            // Arrange
            Wall wall = new Wall(1, 1, 3, 1);

            // Act
            bool result = wall.IsPlaceable();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsPlaceable_Should_Return_False_If_Placeable_A_Vertical_Wall_That_Is_Too_Long()
        {
            // Arrange
            Wall wall = new Wall(1, 1, 1, 3);

            // Act
            bool result = wall.IsPlaceable();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetEndPosition_Should_Return_Correct_End_Position()
        {
            // Arrange
            Wall wall = new Wall(1, 1, 2, 1);
            Position expectedEndPosition = new Position(2, 1);

            // Act
            Position actualEndPosition = wall.GetEndPosition();

            // Assert
            Assert.Equal(expectedEndPosition.GetPositionX(), actualEndPosition.GetPositionX());
            Assert.Equal(expectedEndPosition.GetPositionY(), actualEndPosition.GetPositionY());
        }

        [Fact]
        public void SetEndPosition_Should_Update_End_Position_Correctly()
        {
            // Arrange
            Wall wall = new Wall(1, 1, 2, 1);
            Position expectedEndPosition = new Position(3, 1);

            // Act
            wall.SetEndPosition(3, 1);
            Position actualEndPosition = wall.GetEndPosition();

            // Assert
            Assert.Equal(expectedEndPosition.GetPositionX(), actualEndPosition.GetPositionX());
            Assert.Equal(expectedEndPosition.GetPositionY(), actualEndPosition.GetPositionY());
        }
    }
}