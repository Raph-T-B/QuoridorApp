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
    static class Program
    {
        private static readonly GameManager _gameManager = new(new StubLoadManager(), new StubSaveManager());

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
                    Console.Write(" ");
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
                if ((wall.p1.GetPositionX() == x + 1 && wall.p1.GetPositionY() == y) ||
                    (wall.p2.GetPositionX() == x + 1 && wall.p2.GetPositionY() == y))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("|");
                    Console.ResetColor();
                    isWall = true;
                    break;
                }
            }
            if (!isWall)
            {
                Console.Write(" ");
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
                    Console.Write("-");
                    Console.ResetColor();
                    isWall = true;
                    break;
                }
            }
            if (!isWall)
            {
                Console.Write(" ");
            }
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
        static void Main()
        {
            Console.WriteLine("=== Bienvenue dans Quoridor ===");
            InitializeGame();
            RunGameLoop();
        }

        private static void InitializeGame()
        {
            _gameManager.InitGame(new Player("Player1"), new Player("Player2"));

            Console.WriteLine("\n=== Partie initialisée ===");
            Console.WriteLine($"Joueur 1: {_gameManager.GetPlayers()[0].Name}");
            Console.WriteLine($"Joueur 2: {_gameManager.GetPlayers()[1].Name}");
        }

        private static void RunGameLoop()
        {
            while (!_gameManager.IsGameFinished())
            {
                var currentRound = _gameManager.GetCurrentRound();
                if (currentRound == null) continue;

                DisplayCurrentGameState(currentRound);
                HandlePlayerTurn(currentRound);
            }

            DisplayGameOver();
        }

        private static void DisplayCurrentGameState(Round currentRound)
        {
            var currentPlayer = _gameManager.GetCurrentPlayer();
            var players = _gameManager.GetPlayers();
            var isPlayer1 = players[0] == currentPlayer;
            var playerColor = isPlayer1 ? ConsoleColor.Blue : ConsoleColor.Red;

            DisplayBoard(currentRound.GetBoard());
            DisplayMenu(currentPlayer, playerColor);
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

        private static void HandlePlayerTurn(Round currentRound)
        {
            var currentPlayer = _gameManager.GetCurrentPlayer();
            var players = _gameManager.GetPlayers();
            var isPlayer1 = players[0] == currentPlayer;
            var playerColor = isPlayer1 ? ConsoleColor.Blue : ConsoleColor.Red;

            string? choice = Console.ReadLine();
            if (choice == null) return;

            ProcessPlayerChoice(choice, currentRound, playerColor);
        }

        private static void ProcessPlayerChoice(string choice, Round currentRound, ConsoleColor playerColor)
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
                    HandleDisplayGameState(playerColor);
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
            var currentPlayer = _gameManager.GetCurrentPlayer();
            var board = currentRound.GetBoard();
            var pawn = currentPlayer == _gameManager.GetPlayers()[0] ? board.Pawn1 : board.Pawn2;
            var possibleMoves = board.GetPossibleMoves(pawn);

            Console.ForegroundColor = playerColor;
            Console.WriteLine("\nMouvements possibles :");
            foreach (var move in possibleMoves)
            {
                Console.Write($"({move.GetPositionX()} {move.GetPositionY()}) ");
            }
            Console.WriteLine("\nEntrez les coordonnées du déplacement (x y) :");
            Console.ResetColor();
            
            string? moveInput = Console.ReadLine();
            if (moveInput == null) return;

            ExecuteMove(currentRound, currentPlayer, moveInput, playerColor);
        }

        private static void ExecuteMove(Round currentRound, Player? currentPlayer, string moveInput, ConsoleColor playerColor)
        {
            string[] coords = moveInput.Split(' ');
            if (coords.Length != 2 || !int.TryParse(coords[0], out int x) || !int.TryParse(coords[1], out int y))
            {
                DisplayError("Format invalide. Utilisez 'x y' (ex: 4 5)", playerColor);
                return;
            }

            try
            {
                bool success = currentRound.MovePawn(x, y);
                if (!success)
                {
                    DisplayError("Mouvement invalide. Vérifiez que la case est adjacente et accessible.", playerColor);
                    return;
                }

                if ((currentPlayer == _gameManager.GetPlayers()[0] && x == 8) || 
                    (currentPlayer == _gameManager.GetPlayers()[1] && x == 0))
                {
                    HandleWinningMove(currentPlayer, playerColor);
                }
                else
                {
                    _gameManager.PlayTurn();
                }
            }
            catch (Exception ex)
            {
                DisplayError($"Erreur lors du déplacement : {ex.Message}", playerColor);
            }
        }

        private static void HandleWinningMove(Player? currentPlayer, ConsoleColor playerColor)
        {
            System.Threading.Thread.Sleep(100);
            
            var bestOf = _gameManager.GetBestOf();
            Console.ForegroundColor = playerColor;
            Console.WriteLine($"\n=== Le joueur {currentPlayer?.Name} a gagné la manche ! ===");
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

        private static void HandlePlaceWall(Round currentRound, ConsoleColor playerColor)
        {
            Console.ForegroundColor = playerColor;
            Console.WriteLine("\nEntrez les coordonnées du mur (x y) et son orientation (h pour horizontal, v pour vertical) :");
            Console.WriteLine("Exemple: 4 5 h pour un mur horizontal à la position (4,5)");
            Console.ResetColor();
            
            string? wallInput = Console.ReadLine();
            if (wallInput == null) return;

            if (!TryParseWallCoordinates(wallInput, out int x, out int y, out string orientation))
            {
                DisplayError("Format invalide. Utilisez 'x y h' pour un mur horizontal ou 'x y v' pour un mur vertical (ex: 4 5 h)", playerColor);
                return;
            }

            TryPlaceWall(currentRound, x, y, orientation, playerColor);
        }

        private static bool TryParseWallCoordinates(string wallInput, out int x, out int y, out string orientation)
        {
            x = 0;
            y = 0;
            orientation = "";
            string[] parts = wallInput.Split(' ');
            
            if (parts.Length != 3 || 
                !int.TryParse(parts[0], out x) || 
                !int.TryParse(parts[1], out y) || 
                !IsValidOrientation(parts[2]))
            {
                return false;
            }

            orientation = string.Equals(parts[2], "h", StringComparison.OrdinalIgnoreCase) ? "horizontal" : "vertical";
            return true;
        }

        private static bool IsValidOrientation(string orientation)
        {
            return string.Equals(orientation, "h", StringComparison.OrdinalIgnoreCase) || 
                   string.Equals(orientation, "v", StringComparison.OrdinalIgnoreCase);
        }

        private static void TryPlaceWall(Round currentRound, int x, int y, string orientation, ConsoleColor playerColor)
        {
            try
            {
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

        private static void HandleDisplayGameState(ConsoleColor playerColor)
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