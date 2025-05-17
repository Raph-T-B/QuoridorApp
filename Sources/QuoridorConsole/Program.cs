using QuoridorLib.Interfaces;
using QuoridorLib.Managers;
using QuoridorLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace QuoridorConsole
{
    class Program
    {
        private static GameManager _gameManager;
        private static ILoadManager _loadManager;
        private static ISaveManager _saveManager;

        static Program()
        {
            _loadManager = new StubLoadManager();
            _saveManager = new StubSaveManager();
            _gameManager = new GameManager(_loadManager, _saveManager);
        }

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
            for (int y = 0; y < size; y++)
            {
                DisplayRow(y, size, pawns, walls);
            }
        }

        private static void DisplayRow(int y, int size, Dictionary<Player, Position> pawns, List<(Position p1, Position p2)> walls)
        {
            Console.Write($"{y} ");
            for (int x = 0; x < size; x++)
            {
                DisplayCell(x, y, pawns);
                if (x < size - 1)
                {
                    DisplayVerticalWall(x, y, walls);
                }
            }
            Console.WriteLine();

            if (y < size - 1)
            {
                Console.Write("  ");
                for (int x = 0; x < size; x++)
                {
                    DisplayHorizontalWall(x, y, walls);
                    Console.Write("  ");
                }
                Console.WriteLine();
            }
        }

        private static void DisplayCell(int x, int y, Dictionary<Player, Position> pawns)
        {
            bool isPawn = false;
            foreach (var pawn in pawns)
            {
                if (pawn.Value.GetPositionX() == x && pawn.Value.GetPositionY() == y)
                {
                    // Le premier joueur dans le dictionnaire est toujours le joueur 1 (bleu)
                    bool isPlayer1 = pawns.Keys.First() == pawn.Key;
                    Console.ForegroundColor = isPlayer1 ? ConsoleColor.Blue : ConsoleColor.Red;
                    Console.Write(isPlayer1 ? "1 " : "2 ");
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
            InitializeGame();
            RunGameLoop();
        }

        private static void InitializeGame()
        {
            _loadManager = new StubLoadManager();
            _saveManager = new StubSaveManager();
            _gameManager = new GameManager(_loadManager, _saveManager);

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

            var player1 = new Player(player1Name);
            var player2 = new Player(player2Name);
            _gameManager.InitGame(player1, player2);

            Console.WriteLine("\n=== Partie initialisée ===");
            Console.WriteLine($"Joueur 1: {player1Name}");
            Console.WriteLine($"Joueur 2: {player2Name}");
            Console.WriteLine($"Nombre de parties: {numberOfGames}");
        }

        private static void RunGameLoop()
        {
            while (!_gameManager.IsGameFinished())
            {
                var currentRound = _gameManager.GetCurrentRound();
                if (currentRound == null) continue;

                var currentPlayer = _gameManager.GetCurrentPlayer();
                var players = _gameManager.GetPlayers();
                var isPlayer1 = players[0] == currentPlayer;
                var playerColor = isPlayer1 ? ConsoleColor.Blue : ConsoleColor.Red;

                DisplayBoard(currentRound.GetBoard());
                DisplayMenu(currentPlayer, playerColor);
                HandleUserChoice(Console.ReadLine(), currentRound, playerColor);
            }

            DisplayGameOver();
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

        private static void HandleUserChoice(string? choice, Round currentRound, ConsoleColor playerColor)
        {
            switch (choice)
            {
                case "1":
                    HandleMovePawn(currentRound, playerColor);
                    break;
                case "2":
                    HandlePlaceWall(currentRound, playerColor);
                    break;
                case "3":
                    HandleSaveGame(playerColor);
                    break;
                case "4":
                    HandleLoadGame(playerColor);
                    break;
                case "5":
                    HandleDisplayGameState(currentRound, playerColor);
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

        private static void HandleMovePawn(Round currentRound, ConsoleColor playerColor)
        {
            Console.ForegroundColor = playerColor;
            Console.WriteLine("\nEntrez les coordonnées du déplacement (x y) :");
            Console.ResetColor();
            
            string? moveInput = Console.ReadLine();
            if (moveInput != null)
            {
                string[] coords = moveInput.Split(' ');
                if (coords.Length == 2 && int.TryParse(coords[0], out int x) && int.TryParse(coords[1], out int y))
                {
                    try
                    {
                        bool success = currentRound.MovePawn(x, y);
                        if (success)
                        {
                            _gameManager.PlayTurn();
                            var bestOf = _gameManager.GetBestOf();
                            if (bestOf.GetPlayer1Score() > 0 || bestOf.GetPlayer2Score() > 0)
                            {
                                Console.ForegroundColor = playerColor;
                                Console.WriteLine($"\n=== Le joueur {_gameManager.GetCurrentPlayer()?.Name} a gagné la manche ! ===");
                                Console.ResetColor();
                                Console.WriteLine($"Score actuel - Joueur 1: {bestOf.GetPlayer1Score()}, Joueur 2: {bestOf.GetPlayer2Score()}");
                                
                                if (_gameManager.IsGameFinished())
                                {
                                    DisplayGameOver();
                                }
                                else
                                {
                                    Console.WriteLine("\nAppuyez sur Entrée pour commencer une nouvelle manche...");
                                    Console.ReadLine();
                                    Console.WriteLine("\n=== Nouvelle manche ! ===");
                                    var players = _gameManager.GetPlayers();
                                    _gameManager.InitGame(players[0], players[1]);
                                }
                            }
                        }
                        else
                        {
                            DisplayError("Mouvement invalide. Vérifiez que la case est adjacente et accessible.", playerColor);
                        }
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

        private static void HandlePlaceWall(Round currentRound, ConsoleColor playerColor)
        {
            Console.ForegroundColor = playerColor;
            Console.WriteLine("\nEntrez les coordonnées du mur (x y) et son orientation (h pour horizontal, v pour vertical) :");
            Console.WriteLine("Exemple: 4 5 h pour un mur horizontal à la position (4,5)");
            Console.ResetColor();
            
            string? wallInput = Console.ReadLine();
            if (wallInput != null)
            {
                string[] parts = wallInput.Split(' ');
                if (parts.Length == 3 && 
                    int.TryParse(parts[0], out int x) && 
                    int.TryParse(parts[1], out int y) && 
                    (parts[2].ToLower() == "h" || parts[2].ToLower() == "v"))
                {
                    try
                    {
                        string orientation = parts[2].ToLower() == "h" ? "horizontal" : "vertical";
                        
                        if (currentRound.PlacingWall(x, y, orientation))
                        {
                            _gameManager.PlayTurn();
                        }
                        else
                        {
                            DisplayError("Placement de mur invalide. Vérifiez qu'il n'y a pas de mur qui se croise ou qui se chevauche.", playerColor);
                        }
                    }
                    catch (Exception ex)
                    {
                        DisplayError($"Erreur lors du placement du mur : {ex.Message}", playerColor);
                    }
                }
                else
                {
                    DisplayError("Format invalide. Utilisez 'x y h' pour un mur horizontal ou 'x y v' pour un mur vertical (ex: 4 5 h)", playerColor);
                }
            }
        }

        private static void HandleSaveGame(ConsoleColor playerColor)
        {
            try
            {
                _gameManager.SaveGame();
                Console.ForegroundColor = playerColor;
                Console.WriteLine("Partie sauvegardée avec succès.");
                Console.ResetColor();
            }
            catch (NotSupportedException ex)
            {
                DisplayError($"Erreur lors de la sauvegarde : {ex.Message}", playerColor);
            }
        }

        private static void HandleLoadGame(ConsoleColor playerColor)
        {
            try
            {
                _gameManager.LoadGameState();
                Console.ForegroundColor = playerColor;
                Console.WriteLine("Partie chargée avec succès.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                DisplayError($"Erreur lors du chargement : {ex.Message}", playerColor);
            }
        }

        private static void HandleDisplayGameState(Round currentRound, ConsoleColor playerColor)
        {
            var bestOf = _gameManager.GetBestOf();
            Console.ForegroundColor = playerColor;
            Console.WriteLine("=== État du jeu ===");
            Console.WriteLine($"Joueur actuel : {_gameManager.GetCurrentPlayer()?.Name ?? "Aucun"}");
            Console.WriteLine($"Score - Joueur 1 : {bestOf.GetPlayer1Score()}");
            Console.WriteLine($"Score - Joueur 2 : {bestOf.GetPlayer2Score()}");
            Console.ResetColor();
        }

        private static void DisplayGameOver()
        {
            var bestOf = _gameManager.GetBestOf();
            Console.WriteLine("\n=== Partie terminée ===");
            Console.WriteLine($"Score final:");
            Console.WriteLine($"Joueur 1: {bestOf.GetPlayer1Score()}");
            Console.WriteLine($"Joueur 2: {bestOf.GetPlayer2Score()}");
            Console.WriteLine("=====================");
            Console.WriteLine("\nAppuyez sur Entrée pour quitter...");
            Console.ReadLine();
            Environment.Exit(0);
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
            var game = new Game();
            game.AddPlayer(new Player("Player1"));
            game.AddPlayer(new Player("Player2"));
            game.LaunchRound();
            return game;
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
            throw new NotSupportedException("SaveGame n'est pas implémenté dans le stub");
        }

        public void SaveGameState(GameState gameState)
        {
            throw new NotSupportedException("SaveGameState n'est pas implémenté dans le stub");
        }
    }
}