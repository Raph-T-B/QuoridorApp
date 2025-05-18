// See https://aka.ms/new-console-template for more information

using QuoridorLib.Interfaces;
using QuoridorLib.Managers;
using QuoridorLib.Models;
using System;

namespace QuoridorConsole
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// Cette méthode initialise le jeu et gère la boucle principale :
        /// 1. Crée les joueurs
        /// 2. Initialise les managers (load/save)
        /// 3. Configure les événements du jeu
        /// 4. Lance la partie
        /// 5. Exécute la boucle de jeu jusqu'à la fin
        /// </summary>
        /// <param name="args">Arguments de ligne de commande (non utilisés)</param>
        static void Main(string[] args)
        {
            // Création des joueurs
            Player player1 = new ("Joueur 1");
            Player player2 = new ("Joueur 2");

            // Initialisation des managers
            ILoadManager loadManager = new StubLoadManager();
            ISaveManager saveManager = new StubSaveManager();
            GameManager gameManager = new (loadManager, saveManager);
            
            gameManager.GameInitialized += (sender, players) =>
            {
                Console.WriteLine("=== Nouvelle partie initialisée ===");
                Console.WriteLine($"Joueur 1: {players.player1.Name}");
                Console.WriteLine($"Joueur 2: {players.player2.Name}");
                Console.WriteLine("=================================");
            };

            gameManager.TurnStarted += (sender, player) =>
            {
                Console.WriteLine($"\n=== Tour de {player.Name} ===");
            };

            gameManager.TurnEnded += (sender, nextPlayer) =>
            {
                Console.WriteLine($"=== Fin du tour, prochain joueur: {nextPlayer.Name} ===");
            };

            gameManager.GameFinished += (sender, bestOf) =>
            {
                Console.WriteLine("\n=== Partie terminée ===");
                Console.WriteLine($"Score final:");
                Console.WriteLine($"Joueur 1: {bestOf.GetPlayer1Score()}");
                Console.WriteLine($"Joueur 2: {bestOf.GetPlayer2Score()}");
                Console.WriteLine("=====================");
            };

            gameManager.GameStateChanged += (sender, gameState) =>
            {
                Console.WriteLine("\nÉtat du jeu mis à jour");
            };

            // Initialisation de la partie
            gameManager.InitGame(player1, player2);

            // Boucle principale du jeu
            while (!gameManager.IsGameFinished())
            {
                // Jouer un tour
                gameManager.PlayTurn();
            }
        }
    }

    // Classes de test (stubs) pour les managers
    public class StubLoadManager : ILoadManager
    {
        public Game LoadGame()
        {
            return new Game();
        }

        public GameState LoadGameState()
        {
            return new GameState();
        }
    }

    public class StubSaveManager : ISaveManager
    {
        public void SaveGame(Game game)
        {
            // Cette méthode est intentionnellement vide car c'est un stub utilisé uniquement pour les tests
            // En production, cette méthode devrait sauvegarder l'état du jeu
            throw new NotSupportedException("SaveGame n'est pas implémenté dans le stub");
        }

        public void SaveGameState(GameState gameState)
        {
            // Cette méthode est intentionnellement vide car c'est un stub utilisé uniquement pour les tests
            // En production, cette méthode devrait sauvegarder l'état complet du jeu
            throw new NotSupportedException("SaveGameState n'est pas implémenté dans le stub");
        }
    }
}