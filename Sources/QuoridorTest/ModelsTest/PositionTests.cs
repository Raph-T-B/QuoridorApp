using Xunit;
using QuoridorLib.Models;

namespace QuoridorTest.ModelsTest
{
    public class PositionTests
    {
        [Fact]
        public void Constructor_WithCoordinates_ShouldSetCorrectValues()
        {
            // Arrange & Act
            Position position = new Position(3, 4);

            // Assert
            Assert.Equal(3, position.X);
            Assert.Equal(4, position.Y);
        }

        [Fact]
        public void Constructor_WithPosition_ShouldCreateCopy()
        {
            // Arrange
            Position original = new Position(3, 4);

            // Act
            Position copy = new Position(original);

            // Assert
            Assert.Equal(original.X, copy.X);
            Assert.Equal(original.Y, copy.Y);
            Assert.NotSame(original, copy); // Vérifie que c'est une copie et non la même référence
        }

        [Fact]
        public void SetPosition_ShouldUpdateCoordinates()
        {
            // Arrange
            Position position = new Position(1, 1);

            // Act
            position.SetPosition(5, 6);

            // Assert
            Assert.Equal(5, position.X);
            Assert.Equal(6, position.Y);
        }

        [Fact]
        public void GetPosition_ShouldReturnSelf()
        {
            // Arrange
            Position position = new Position(3, 4);

            // Act
            Position result = position.GetPosition();

            // Assert
            Assert.Same(position, result);
        }

        [Fact]
        public void GetPositionX_ShouldReturnX()
        {
            // Arrange
            Position position = new Position(3, 4);

            // Act
            int x = position.GetPositionX();

            // Assert
            Assert.Equal(3, x);
        }

        [Fact]
        public void GetPositionY_ShouldReturnY()
        {
            // Arrange
            Position position = new Position(3, 4);

            // Act
            int y = position.GetPositionY();

            // Assert
            Assert.Equal(4, y);
        }

        [Fact]
        public void Equals_WithSamePosition_ShouldReturnTrue()
        {
            // Arrange
            Position pos1 = new Position(3, 4);
            Position pos2 = new Position(3, 4);

            // Act & Assert
            Assert.True(pos1.Equals(pos2));
        }

        [Fact]
        public void Equals_WithDifferentPosition_ShouldReturnFalse()
        {
            // Arrange
            Position pos1 = new Position(3, 4);
            Position pos2 = new Position(3, 5);

            // Act & Assert
            Assert.False(pos1.Equals(pos2));
        }

        [Fact]
        public void Equals_WithNull_ShouldReturnFalse()
        {
            // Arrange
            Position position = new Position(3, 4);

            // Act & Assert
            Assert.False(position.Equals(null));
        }

        [Fact]
        public void Equals_WithDifferentType_ShouldReturnFalse()
        {
            // Arrange
            Position position = new Position(3, 4);
            object other = "not a position";

            // Act & Assert
            Assert.False(position.Equals(other));
        }

        [Fact]
        public void GetHashCode_WithSamePosition_ShouldReturnSameHash()
        {
            // Arrange
            Position pos1 = new Position(3, 4);
            Position pos2 = new Position(3, 4);

            // Act & Assert
            Assert.Equal(pos1.GetHashCode(), pos2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_WithDifferentPosition_ShouldReturnDifferentHash()
        {
            // Arrange
            Position pos1 = new Position(3, 4);
            Position pos2 = new Position(3, 5);

            // Act & Assert
            Assert.NotEqual(pos1.GetHashCode(), pos2.GetHashCode());
        }
    }
} 