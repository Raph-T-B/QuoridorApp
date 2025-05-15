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
            
            DisplayHeader();
            DisplayBoardContent(SIZE, pawns, walls);
            Console.WriteLine();
        }

        private static void DisplayHeader()
        {
            Console.WriteLine("\n  0   1   2   3   4   5   6   7   8");
        }

        private static void DisplayBoardContent(int size, Dictionary<Player, Position> pawns, List<(Position p1, Position p2)> walls)
        {
            for (int y = 0; y < size + size - 1; y++)
            {
                if (y % 2 == 0)
                {
                    DisplayEvenRow(y, size, pawns, walls);
                }
                else
                {
                    DisplayOddRow(y, size, walls);
                }
            }
        }

        private static void DisplayEvenRow(int y, int size, Dictionary<Player, Position> pawns, List<(Position p1, Position p2)> walls)
        {
            Console.Write($"{y/2} ");
            for (int x = 0; x < size + size - 1; x++)
            {
                if (x % 2 == 0 || y % 2 == 0)
                {
                    DisplayCell(x/2, y/2, pawns);
                }
                else
                {
                    DisplayVerticalWall(x/2, y/2, walls);
                }
            }
            Console.WriteLine();
        }

        private static void DisplayOddRow(int y, int size, List<(Position p1, Position p2)> walls)
        {
            Console.Write("  ");
            for (int x = 0; x < size; x++)
            {
                if (x % 2 == 0)
                {
                    DisplayHorizontalWall(x/2, y/2, walls);
                }
                else
                {
                    Console.Write("  ");
                }
            }
            Console.WriteLine();
        }

        private static void DisplayCell(int x, int y, Dictionary<Player, Position> pawns)
        {
            bool isPawn = false;
            foreach (var pawn in pawns)
            {
                if (pawn.Value.GetPositionX() == x && pawn.Value.GetPositionY() == y)
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

        private static void DisplayVerticalWall(int x, int y, List<(Position p1, Position p2)> walls)
        {
            bool isWall = false;
            foreach (var wall in walls)
            {
                if ((wall.p1.GetPositionX() == x && wall.p1.GetPositionY() == y) ||
                    (wall.p2.GetPositionX() == x && wall.p2.GetPositionY() == y))
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

        private static void DisplayHorizontalWall(int x, int y, List<(Position p1, Position p2)> walls)
        {
            bool isWall = false;
            foreach (var wall in walls)
            {
                if ((wall.p1.GetPositionX() == x && wall.p1.GetPositionY() == y) ||
                    (wall.p2.GetPositionX() == x && wall.p2.GetPositionY() == y))
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
            
            var (player1, player2, _) = GetGameConfiguration();
            var gameManager = InitializeGameManager(player1, player2);
            
            RunGameLoop(gameManager);
        }

        private static GameManager InitializeGameManager(Player player1, Player player2)
        {
            ILoadManager loadManager = new StubLoadManager();
            ISaveManager saveManager = new StubSaveManager();
            GameManager gameManager = new GameManager(loadManager, saveManager);
            
            gameManager.GameInitialized += OnGameInitialized;
            gameManager.TurnStarted += OnTurnStarted;
            gameManager.TurnEnded += OnTurnEnded;
            gameManager.GameFinished += OnGameFinished;
            gameManager.GameStateChanged += OnGameStateChanged;

            gameManager.InitGame(player1, player2);
            return gameManager;
        }

        private static void RunGameLoop(GameManager gameManager)
        {
            while (!gameManager.IsGameFinished())
            {
                var currentRound = gameManager.GetCurrentRound();
                if (currentRound == null) continue;

                var currentPlayer = gameManager.GetCurrentPlayer();
                var players = gameManager.GetPlayers();
                var isPlayer1 = players[0] == currentPlayer;
                var playerColor = isPlayer1 ? ConsoleColor.Blue : ConsoleColor.Red;

                DisplayMenu(currentPlayer, playerColor);
                HandleUserChoice(Console.ReadLine(), currentRound, gameManager, playerColor);
            }
        }

        private static void DisplayMenu(Player? currentPlayer, ConsoleColor playerColor)
        {
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
        }

        private static void HandleUserChoice(string? choice, Round currentRound, GameManager gameManager, ConsoleColor playerColor)
        {
            switch (choice)
            {
                case "1":
                    HandleMovePawn(currentRound, gameManager, playerColor);
                    break;
                case "2":
                    HandlePlaceWall(currentRound, gameManager, playerColor);
                    break;
                case "3":
                    HandleSaveGame(gameManager, playerColor);
                    break;
                case "4":
                    HandleLoadGame(gameManager, playerColor);
                    break;
                case "5":
                    HandleDisplayGameState(currentRound, gameManager, playerColor);
                    break;
                case "6":
                    Environment.Exit(0);
                    break;
                default:
                    Console.ForegroundColor = playerColor;
                    Console.WriteLine("Choix invalide. Veuillez réessayer.");
                    Console.ResetColor();
                    break;
            }
        }

        private static void HandleMovePawn(Round currentRound, GameManager gameManager, ConsoleColor playerColor)
        {
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
                        DisplayError($"Erreur lors du déplacement : {ex.Message}", playerColor);
                    }
                }
                else
                {
                    DisplayError("Format invalide. Utilisez 'x y' (ex: 4 5)", playerColor);
                }
            }
        }

        private static void HandlePlaceWall(Round currentRound, GameManager gameManager, ConsoleColor playerColor)
        {
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
                            DisplayError("Placement de mur invalide", playerColor);
                        }
                    }
                    catch (Exception ex)
                    {
                        DisplayError($"Erreur lors du placement du mur : {ex.Message}", playerColor);
                    }
                }
                else
                {
                    DisplayError("Format invalide. Utilisez 'x y' (ex: 4 5)", playerColor);
                }
            }
        }

        private static void HandleSaveGame(GameManager gameManager, ConsoleColor playerColor)
        {
            try
            {
                gameManager.SaveGame();
                Console.ForegroundColor = playerColor;
                Console.WriteLine("Partie sauvegardée avec succès.");
                Console.ResetColor();
            }
            catch (NotSupportedException ex)
            {
                DisplayError($"Erreur lors de la sauvegarde : {ex.Message}", playerColor);
            }
        }

        private static void HandleLoadGame(GameManager gameManager, ConsoleColor playerColor)
        {
            try
            {
                gameManager.LoadGameState();
                Console.ForegroundColor = playerColor;
                Console.WriteLine("Partie chargée avec succès.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                DisplayError($"Erreur lors du chargement : {ex.Message}", playerColor);
            }
        }

        private static void HandleDisplayGameState(Round currentRound, GameManager gameManager, ConsoleColor playerColor)
        {
            var bestOf = gameManager.GetBestOf();
            Console.ForegroundColor = playerColor;
            Console.WriteLine("=== État du jeu ===");
            Console.WriteLine($"Joueur actuel : {gameManager.GetCurrentPlayer()?.Name ?? "Aucun"}");
            Console.WriteLine($"Score - Joueur 1 : {bestOf.GetPlayer1Score()}");
            Console.WriteLine($"Score - Joueur 2 : {bestOf.GetPlayer2Score()}");
            Console.ResetColor();
            DisplayBoard(currentRound.GetBoard());
        }

        private static void DisplayError(string message, ConsoleColor playerColor)
        {
            Console.ForegroundColor = playerColor;
            Console.WriteLine(message);
            Console.ResetColor();
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