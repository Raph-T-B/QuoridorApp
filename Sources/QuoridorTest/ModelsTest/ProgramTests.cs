using QuoridorLib.Models;
using Xunit;

namespace QuoridorLib.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void TestGameVictory()
        {
            // Créer les joueurs
            var player1 = new Player("Joueur 1");
            var player2 = new Player("Joueur 2");

            // Créer le jeu
            var game = new Game();
            game.AddPlayer(player1);
            game.AddPlayer(player2);
            game.LaunchRound();

            // Simuler une victoire du joueur 1
            var round = game.GetCurrentRound();
            Assert.NotNull(round);

            // Déplacer le joueur 1 jusqu'à la victoire (x=8)
            round.MovePawn(8, 4); // Déplacement direct à la ligne de victoire

            // Vérifier que le joueur 1 a gagné
            Assert.Equal(1, game.GetBestOf().GetPlayer1Score());
            Assert.Equal(0, game.GetBestOf().GetPlayer2Score());

            // Simuler une deuxième victoire du joueur 1
            game.LaunchRound();
            round = game.GetCurrentRound();
            Assert.NotNull(round);

            // Déplacer le joueur 1 jusqu'à la victoire (x=8)
            round.MovePawn(8, 4); // Déplacement direct à la ligne de victoire

            // Vérifier que le joueur 1 a gagné la partie
            Assert.Equal(2, game.GetBestOf().GetPlayer1Score());
            Assert.Equal(0, game.GetBestOf().GetPlayer2Score());
            Assert.True(game.IsGameOver());
        }

        [Fact]
        public void TestAlternateVictories()
        {
            // Créer les joueurs
            var player1 = new Player("Joueur 1");
            var player2 = new Player("Joueur 2");

            // Créer le jeu
            var game = new Game();
            game.AddPlayer(player1);
            game.AddPlayer(player2);
            game.LaunchRound();

            // Première manche : Victoire du joueur 1
            var round = game.GetCurrentRound();
            Assert.NotNull(round);
            round.MovePawn(8, 4);
            Assert.Equal(1, game.GetBestOf().GetPlayer1Score());
            Assert.Equal(0, game.GetBestOf().GetPlayer2Score());

            // Deuxième manche : Victoire du joueur 2
            game.LaunchRound();
            round = game.GetCurrentRound();
            Assert.NotNull(round);
            round.MovePawn(0, 4);
            Assert.Equal(1, game.GetBestOf().GetPlayer1Score());
            Assert.Equal(1, game.GetBestOf().GetPlayer2Score());

            // Troisième manche : Victoire du joueur 1
            game.LaunchRound();
            round = game.GetCurrentRound();
            Assert.NotNull(round);
            round.MovePawn(8, 4);
            Assert.Equal(2, game.GetBestOf().GetPlayer1Score());
            Assert.Equal(1, game.GetBestOf().GetPlayer2Score());
            Assert.True(game.IsGameOver());
        }
    }
} 