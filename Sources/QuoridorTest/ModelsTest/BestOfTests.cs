using Xunit;
using QuoridorLib.Models;

namespace QuoridorTest.ModelsTest
{
    public class BestOfTests
    {
        [Fact]
        public void Constructor_Should_Initialize_Scores_To_Zero()
        {
            // Arrange & Act
            BestOf bestOf = new (3);

            // Assert
            Assert.Equal(0, bestOf.GetPlayer1Score());
            Assert.Equal(0, bestOf.GetPlayer2Score());
        }

        [Fact]
        public void AddPlayer1Victory_Should_Increment_Player1Score()
        {
            // Arrange
            BestOf bestOf = new (3);

            // Act
            bestOf.AddPlayer1Victory();

            // Assert
            Assert.Equal(1, bestOf.GetPlayer1Score());
            Assert.Equal(0, bestOf.GetPlayer2Score());
        }

        [Fact]
        public void AddPlayer2Victory_Should_Increment_Player2Score()
        {
            // Arrange
            BestOf bestOf = new (3);

            // Act
            bestOf.AddPlayer2Victory();

            // Assert
            Assert.Equal(0, bestOf.GetPlayer1Score());
            Assert.Equal(1, bestOf.GetPlayer2Score());
        }

        [Fact]
        public void Multiple_Victories_Should_Accumulate_Correctly()
        {
            // Arrange
            BestOf bestOf = new (5);

            // Act
            bestOf.AddPlayer1Victory();
            bestOf.AddPlayer1Victory();
            bestOf.AddPlayer2Victory();
            bestOf.AddPlayer1Victory();

            // Assert
            Assert.Equal(3, bestOf.GetPlayer1Score());
            Assert.Equal(1, bestOf.GetPlayer2Score());
        }

        

        [Fact]
        public void GetNumberOfGames_Should_Return_Correct_Number()
        {
            // Arrange
            int expectedGames = 5;
            BestOf bestOf = new (expectedGames);

            // Act
            int actualGames = bestOf.GetNumberOfGames();

            // Assert
            Assert.Equal(expectedGames, actualGames);
        }
    }
} 