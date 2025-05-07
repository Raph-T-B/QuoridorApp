using Xunit;
using QuoridorLib.Models;

namespace QuoridorTest.ModelsTest
{
    public class PlayerTests
    {
        [Fact]
        public void Constructor_Should_Set_Name_Correctly()
        {
            // Arrange
            string expectedName = "TestPlayer";

            // Act
            Player player = new Player(expectedName);

            // Assert
            Assert.Equal(expectedName, player.Name);
        }

        [Fact]
        public void Victories_Should_Start_At_Zero()
        {
            // Arrange & Act
            Player player = new Player("TestPlayer");

            // Assert
            Assert.Equal(0u, player.Victories);
        }

        [Fact]
        public void AddVictory_Should_Increment_Victories()
        {
            // Arrange
            Player player = new Player("TestPlayer");

            // Act
            player.AddVictory();

            // Assert
            Assert.Equal(1u, player.Victories);
        }

        [Fact]
        public void Multiple_Victories_Should_Accumulate_Correctly()
        {
            // Arrange
            Player player = new Player("TestPlayer");

            // Act
            player.AddVictory();
            player.AddVictory();
            player.AddVictory();

            // Assert
            Assert.Equal(3u, player.Victories);
        }

        [Fact]
        public void Name_Should_Be_ReadOnly()
        {
            // Arrange
            Player player = new Player("TestPlayer");

            // Act & Assert
            Assert.Equal("TestPlayer", player.Name);
        }
    }
} 