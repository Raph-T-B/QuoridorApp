using QuoridorLib.Models;
using Xunit;

namespace QuoridorLib.Tests
{
    public class ProgramTests
    {
        private Game CreateAndInitializeGame()
        {
            var player1 = new Player("Joueur 1");
            var player2 = new Player("Joueur 2");
            var game = new Game();
            game.AddPlayer(player1);
            game.AddPlayer(player2);
            game.LaunchRound();
            return game;
        }

        [Fact]
        public void TestGameVictory()
        {
            // Créer et initialiser le jeu
            var game = CreateAndInitializeGame();
            var round = game.GetCurrentRound();
            Assert.NotNull(round);

            // Vérifier que le joueur 1 est le joueur actuel
            Assert.Equal(game.GetPlayers()[0], round.CurrentPlayerProperty);

            // Déplacer le joueur 1 jusqu'à la victoire (x=8)
            bool victory = round.MovePawn(8, 4); // Déplacement direct à la ligne de victoire
            Assert.True(victory);

            // Vérifier que le joueur 1 a gagné
            Assert.Equal(1, game.GetBestOf().GetPlayer1Score());
            Assert.Equal(0, game.GetBestOf().GetPlayer2Score());

            // Simuler une deuxième victoire du joueur 1
            game.LaunchRound();
            round = game.GetCurrentRound();
            Assert.NotNull(round);

            // Vérifier que le joueur 1 est toujours le joueur actuel
            Assert.Equal(game.GetPlayers()[0], round.CurrentPlayerProperty);

            // Déplacer le joueur 1 jusqu'à la victoire (x=8)
            victory = round.MovePawn(8, 4); // Déplacement direct à la ligne de victoire
            Assert.True(victory);

            // Vérifier que le joueur 1 a gagné la partie
            Assert.Equal(2, game.GetBestOf().GetPlayer1Score());
            Assert.Equal(0, game.GetBestOf().GetPlayer2Score());
            Assert.True(game.IsGameOver());
        }

        [Fact]
        public void TestAlternateVictories()
        {
            // Créer et initialiser le jeu
            var game = CreateAndInitializeGame();
            var round = game.GetCurrentRound();
            Assert.NotNull(round);

            // Première manche : Victoire du joueur 1
            Assert.Equal(game.GetPlayers()[0], round.CurrentPlayerProperty);
            bool victory = round.MovePawn(8, 4);
            Assert.True(victory);
            Assert.Equal(1, game.GetBestOf().GetPlayer1Score());
            Assert.Equal(0, game.GetBestOf().GetPlayer2Score());

            // Deuxième manche : Victoire du joueur 2
            game.LaunchRound();
            round = game.GetCurrentRound();
            Assert.NotNull(round);
            Assert.Equal(game.GetPlayers()[0], round.CurrentPlayerProperty);
            // Changer le joueur actuel pour le joueur 2
            round.SwitchCurrentPlayer(game.GetPlayers()[1]);
            victory = round.MovePawn(0, 4);
            Assert.True(victory);
            Assert.Equal(1, game.GetBestOf().GetPlayer1Score());
            Assert.Equal(1, game.GetBestOf().GetPlayer2Score());

            // Troisième manche : Victoire du joueur 1
            game.LaunchRound();
            round = game.GetCurrentRound();
            Assert.NotNull(round);
            Assert.Equal(game.GetPlayers()[0], round.CurrentPlayerProperty);
            victory = round.MovePawn(8, 4);
            Assert.True(victory);
            Assert.Equal(2, game.GetBestOf().GetPlayer1Score());
            Assert.Equal(1, game.GetBestOf().GetPlayer2Score());
            Assert.True(game.IsGameOver());
        }
    }
} 