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
            BestOf bestOf = new BestOf(3);

            // Assert
            Assert.Equal(0, bestOf.GetScoreJoueur1());
            Assert.Equal(0, bestOf.GetScoreJoueur2());
        }

        [Fact]
        public void AjouterVictoireJoueur1_Should_Increment_ScoreJoueur1()
        {
            // Arrange
            BestOf bestOf = new BestOf(3);

            // Act
            bestOf.AjouterVictoireJoueur1();

            // Assert
            Assert.Equal(1, bestOf.GetScoreJoueur1());
            Assert.Equal(0, bestOf.GetScoreJoueur2());
        }

        [Fact]
        public void AjouterVictoireJoueur2_Should_Increment_ScoreJoueur2()
        {
            // Arrange
            BestOf bestOf = new BestOf(3);

            // Act
            bestOf.AjouterVictoireJoueur2();

            // Assert
            Assert.Equal(0, bestOf.GetScoreJoueur1());
            Assert.Equal(1, bestOf.GetScoreJoueur2());
        }

        [Fact]
        public void Multiple_Victories_Should_Accumulate_Correctly()
        {
            // Arrange
            BestOf bestOf = new BestOf(5);

            // Act
            bestOf.AjouterVictoireJoueur1();
            bestOf.AjouterVictoireJoueur1();
            bestOf.AjouterVictoireJoueur2();
            bestOf.AjouterVictoireJoueur1();

            // Assert
            Assert.Equal(3, bestOf.GetScoreJoueur1());
            Assert.Equal(1, bestOf.GetScoreJoueur2());
        }

        [Fact]
        public void GetScoreJoueur1_Should_Return_2_For_Score()
        {
            // Arrange
            BestOf bestOf = new BestOf(3);
            bestOf.AjouterVictoireJoueur1();
            bestOf.AjouterVictoireJoueur1();

            // Act
            int score = bestOf.GetScoreJoueur1();

            // Assert
            Assert.Equal(2, score);
        }

        [Fact]
        public void GetScoreJoueur2_Should_Return_2_For_Score()
        {
            // Arrange
            BestOf bestOf = new BestOf(3);
            bestOf.AjouterVictoireJoueur2();
            bestOf.AjouterVictoireJoueur2();

            // Act
            int score = bestOf.GetScoreJoueur2();

            // Assert
            Assert.Equal(2, score);
        }
    }
} 