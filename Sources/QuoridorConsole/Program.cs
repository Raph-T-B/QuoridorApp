// See https://aka.ms/new-console-template for more information

using QuoridorLib.Interfaces;
using QuoridorLib.Managers;
using QuoridorLib.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace QuoridorConsole
{
    static class Program
    {
        private static void DisplayBoard(Board board)
        {
            var pawns = board.GetPawnsPositions();
            var walls = board.GetWallsPositions();
            const int SIZE = 9;
            Console.WriteLine("\n  0   1   2   3   4   5   6   7   8");
            for (int y = 0; y < SIZE + SIZE - 1; y++)
            {
                if (y % 2 == 0)
                {
                    Console.Write($"{y/2} ");
                    for (int x = 0; x < SIZE + SIZE - 1; x++)
                    {
                        if (x % 2 == 0 || y % 2 == 0)
                        {
                            // code Board
                            x = x / 2;
                            y = y / 2;
                            bool isPawn = false;
                            foreach (var pawn in pawns)
                            {
                                if (pawn.Value.GetPositionX() == x/2 && pawn.Value.GetPositionY() == y/2)
                                {
                                    Console.ForegroundColor = pawns.Keys.First() == pawn.Key ? ConsoleColor.Blue : ConsoleColor.Red;
                                    Console.Write(pawns.Keys.First() == pawn.Key ? "1 " : "2 ");
                                    Console.ResetColor();
                                    isPawn = true;
                                    break;
                                }
                            }
                            if (!isPawn)
                            {
                                Console.Write(". ");
                            }
                        }
                        else
                        {
                            // code Board
                            x = (x / 2) + 1;
                            y = (y / 2) + 1;
                            bool isWall = false;
                            foreach (var wall in walls)
                            {
                                if ((wall.p1.GetPositionX() == x/2 && wall.p1.GetPositionY() == y/2) ||
                                    (wall.p2.GetPositionX() == x/2 && wall.p2.GetPositionY() == y/2))
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.Write("| ");
                                    Console.ResetColor();
                                    isWall = true;
                                    break;
                                }
                            }
                            if (!isWall)
                            {
                                Console.Write("  ");
                            }
                        }
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.Write("  ");
                    for (int x = 0; x < SIZE; x++)
                    {
                        if (x % 2 == 0)
                        {
                            bool isWall = false;
                            foreach (var wall in walls)
                            {
                                if ((wall.p1.GetPositionX() == x/2 && wall.p1.GetPositionY() == y/2) ||
                                    (wall.p2.GetPositionX() == x/2 && wall.p2.GetPositionY() == y/2))
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.Write("- ");
                                    Console.ResetColor();
                                    isWall = true;
                                    break;
                                }
                            }
                            if (!isWall)
                            {
                                Console.Write("  ");
                            }
                        }
                        else
                        {
                            Console.Write("  ");
                        }
                    }
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }

        private static void OnGameInitialized(object? sender, (Player player1, Player player2) players)
        {
            Console.WriteLine("=== Nouvelle partie initialisée ===");
            Console.WriteLine($"Joueur 1: {players.player1.Name}");
            Console.WriteLine($"Joueur 2: {players.player2.Name}");
            Console.WriteLine("=================================");
        }

        private static void OnTurnStarted(object? sender, Player player)
        {
            Console.WriteLine($"\n=== Tour de {player.Name} ===");
        }

        private static void OnTurnEnded(object? sender, Player nextPlayer)
        {
            Console.WriteLine($"=== Fin du tour, prochain joueur: {nextPlayer.Name} ===");
        }

        private static void OnGameFinished(object? sender, BestOf bestOf)
        {
            Console.WriteLine("\n=== Partie terminée ===");
            Console.WriteLine($"Score final:");
            Console.WriteLine($"Joueur 1: {bestOf.GetPlayer1Score()}");
            Console.WriteLine($"Joueur 2: {bestOf.GetPlayer2Score()}");
            Console.WriteLine("=====================");
        }

        private static void OnGameStateChanged(object? sender, GameState gameState)
        {
            Console.WriteLine("\nÉtat du jeu mis à jour");
            if (gameState.CurrentRound != null)
            {
                DisplayBoard(gameState.CurrentRound.GetBoard());
            }
        }

        private static (Player player1, Player player2, int numberOfGames) GetGameConfiguration()
        {
            Console.WriteLine("\n=== Configuration de la partie ===");
            
            // Demande du nom du joueur 1
            Console.Write("Nom du joueur 1 : ");
            string? player1Name = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(player1Name))
            {
                Console.WriteLine("Le nom ne peut pas être vide.");
                Console.Write("Nom du joueur 1 : ");
                player1Name = Console.ReadLine();
            }

            // Demande du nom du joueur 2
            Console.Write("Nom du joueur 2 : ");
            string? player2Name = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(player2Name))
            {
                Console.WriteLine("Le nom ne peut pas être vide.");
                Console.Write("Nom du joueur 2 : ");
                player2Name = Console.ReadLine();
            }

            // Demande du nombre de parties
            int numberOfGames = 0;
            bool validInput = false;
            while (!validInput)
            {
                Console.Write("Nombre de parties (3, 5 ou 7) : ");
                string? input = Console.ReadLine();
                if (int.TryParse(input, out numberOfGames) && (numberOfGames == 3 || numberOfGames == 5 || numberOfGames == 7))
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Veuillez entrer 3, 5 ou 7.");
                }
            }

            return (new Player(player1Name), new Player(player2Name), numberOfGames);
        }

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
            Console.WriteLine("=== Bienvenue dans Quoridor ===");
            
            // Configuration de la partie
            var (player1, player2, numberOfGames) = GetGameConfiguration();

            // Initialisation des managers
            ILoadManager loadManager = new StubLoadManager();
            ISaveManager saveManager = new StubSaveManager();
            GameManager gameManager = new GameManager(loadManager, saveManager);
            
            gameManager.GameInitialized += OnGameInitialized;
            gameManager.TurnStarted += OnTurnStarted;
            gameManager.TurnEnded += OnTurnEnded;
            gameManager.GameFinished += OnGameFinished;
            gameManager.GameStateChanged += OnGameStateChanged;

            // Initialisation de la partie
            gameManager.InitGame(player1, player2);

            // Boucle principale du jeu
            while (!gameManager.IsGameFinished())
            {
                var currentRound = gameManager.GetCurrentRound();
                if (currentRound == null) continue;

                var currentPlayer = gameManager.GetCurrentPlayer();
                var players = gameManager.GetPlayers();
                var isPlayer1 = players[0] == currentPlayer;
                var playerColor = isPlayer1 ? ConsoleColor.Blue : ConsoleColor.Red;

                Console.ForegroundColor = playerColor;
                Console.WriteLine("\n=== Menu ===");
                Console.WriteLine($"Tour de {currentPlayer?.Name} :");
                Console.WriteLine("1. Déplacer le pion");
                Console.WriteLine("2. Placer un mur");
                Console.WriteLine("3. Sauvegarder la partie");
                Console.WriteLine("4. Charger une partie");
                Console.WriteLine("5. Afficher l'état du jeu");
                Console.WriteLine("6. Quitter");
                Console.Write("Votre choix : ");
                Console.ResetColor();

                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.ForegroundColor = playerColor;
                        Console.WriteLine("Entrez les coordonnées du déplacement (x y) :");
                        Console.ResetColor();
                        string? moveInput = Console.ReadLine();
                        if (moveInput != null)
                        {
                            string[] coords = moveInput.Split(' ');
                            if (coords.Length == 2 && int.TryParse(coords[0], out int x) && int.TryParse(coords[1], out int y))
                            {
                                try
                                {
                                    currentRound.MovePawn(x, y);
                                    gameManager.PlayTurn();
                                    DisplayBoard(currentRound.GetBoard());
                                }
                                catch (Exception ex)
                                {
                                    Console.ForegroundColor = playerColor;
                                    Console.WriteLine($"Erreur lors du déplacement : {ex.Message}");
                                    Console.ResetColor();
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = playerColor;
                                Console.WriteLine("Format invalide. Utilisez 'x y' (ex: 4 5)");
                                Console.ResetColor();
                            }
                        }
                        break;

                    case "2":
                        Console.ForegroundColor = playerColor;
                        Console.WriteLine("Entrez les coordonnées du mur (x y) :");
                        Console.ResetColor();
                        string? wallInput = Console.ReadLine();
                        if (wallInput != null)
                        {
                            string[] coords = wallInput.Split(' ');
                            if (coords.Length == 2 && int.TryParse(coords[0], out int x) && int.TryParse(coords[1], out int y))
                            {
                                try
                                {
                                    if (currentRound.PlacingWall(x, y, "vertical"))
                                    {
                                        gameManager.PlayTurn();
                                        DisplayBoard(currentRound.GetBoard());
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = playerColor;
                                        Console.WriteLine("Placement de mur invalide");
                                        Console.ResetColor();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.ForegroundColor = playerColor;
                                    Console.WriteLine($"Erreur lors du placement du mur : {ex.Message}");
                                    Console.ResetColor();
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = playerColor;
                                Console.WriteLine("Format invalide. Utilisez 'x y' (ex: 4 5)");
                                Console.ResetColor();
                            }
                        }
                        break;

                    case "3":
                        try
                        {
                            gameManager.SaveGame();
                            Console.ForegroundColor = playerColor;
                            Console.WriteLine("Partie sauvegardée avec succès.");
                            Console.ResetColor();
                        }
                        catch (NotSupportedException ex)
                        {
                            Console.ForegroundColor = playerColor;
                            Console.WriteLine($"Erreur lors de la sauvegarde : {ex.Message}");
                            Console.ResetColor();
                        }
                        break;

                    case "4":
                        try
                        {
                            gameManager.LoadGameState();
                            Console.ForegroundColor = playerColor;
                            Console.WriteLine("Partie chargée avec succès.");
                            Console.ResetColor();
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = playerColor;
                            Console.WriteLine($"Erreur lors du chargement : {ex.Message}");
                            Console.ResetColor();
                        }
                        break;

                    case "5":
                        var bestOf = gameManager.GetBestOf();
                        Console.ForegroundColor = playerColor;
                        Console.WriteLine("=== État du jeu ===");
                        Console.WriteLine($"Joueur actuel : {currentPlayer?.Name ?? "Aucun"}");
                        Console.WriteLine($"Score - Joueur 1 : {bestOf.GetPlayer1Score()}");
                        Console.WriteLine($"Score - Joueur 2 : {bestOf.GetPlayer2Score()}");
                        Console.ResetColor();
                        DisplayBoard(currentRound.GetBoard());
                        break;

                    case "6":
                        return;

                    default:
                        Console.ForegroundColor = playerColor;
                        Console.WriteLine("Choix invalide. Veuillez réessayer.");
                        Console.ResetColor();
                        break;
                }
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