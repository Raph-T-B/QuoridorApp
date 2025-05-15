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
            DisplayWall(x, y, walls, "| ");
        }

        private static void DisplayHorizontalWall(int x, int y, List<(Position p1, Position p2)> walls)
        {
            DisplayWall(x, y, walls, "- ");
        }

        private static void DisplayWall(int x, int y, List<(Position p1, Position p2)> walls, string wallSymbol)
        {
            bool isWall = false;
            foreach (var wall in walls)
            {
                if ((wall.p1.GetPositionX() == x && wall.p1.GetPositionY() == y) ||
                    (wall.p2.GetPositionX() == x && wall.p2.GetPositionY() == y))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(wallSymbol);
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
            Console.WriteLine("\nPositions valides pour le déplacement :");
            
            var board = currentRound.GetBoard();
            var currentPawn = gameManager.GetCurrentPlayer() == gameManager.GetPlayers()[0] ? board.Pawn1 : board.Pawn2;
            var currentPosition = currentPawn.GetPawnPosition();
            
            // Vérifier uniquement les positions adjacentes
            int xPawn = currentPosition.GetPositionX();
            int yPawn = currentPosition.GetPositionY();
            
            // Positions possibles (haut, bas, gauche, droite)
            var possiblePositions = new[]
            {
                new Position(xPawn, yPawn - 1), // haut
                new Position(xPawn, yPawn + 1), // bas
                new Position(xPawn - 1, yPawn), // gauche
                new Position(xPawn + 1, yPawn)  // droite
            };

            bool hasValidMoves = false;
            foreach (var pos in possiblePositions)
            {
                // Vérifier si la position est sur le plateau
                if (pos.GetPositionX() < 0 || pos.GetPositionX() >= 9 || 
                    pos.GetPositionY() < 0 || pos.GetPositionY() >= 9)
                    continue;

                // Vérifier si la position est occupée par un autre pion
                if (board.Pawn1.GetPawnPosition().GetPositionX() == pos.GetPositionX() && 
                    board.Pawn1.GetPawnPosition().GetPositionY() == pos.GetPositionY())
                    continue;
                if (board.Pawn2.GetPawnPosition().GetPositionX() == pos.GetPositionX() && 
                    board.Pawn2.GetPawnPosition().GetPositionY() == pos.GetPositionY())
                    continue;

                // Vérifier s'il y a un mur entre les positions
                bool hasWall = false;
                foreach (var wallCouple in board.WallCouples)
                {
                    var wall1 = wallCouple.GetWall1();
                    var wall2 = wallCouple.GetWall2();
                    
                    if (IsWallBlocking(currentPosition, pos, wall1) || IsWallBlocking(currentPosition, pos, wall2))
                    {
                        hasWall = true;
                        break;
                    }
                }
                
                if (!hasWall)
                {
                    Console.Write($"({pos.GetPositionX()}, {pos.GetPositionY()}) ");
                    hasValidMoves = true;
                }
            }

            if (!hasValidMoves)
            {
                Console.WriteLine("Aucun mouvement possible !");
                return;
            }
            
            Console.WriteLine("\n\nEntrez les coordonnées du déplacement (x y) :");
            Console.ResetColor();
            
            string? moveInput = Console.ReadLine();
            if (moveInput != null)
            {
                string[] coords = moveInput.Split(' ');
                if (coords.Length == 2 && int.TryParse(coords[0], out int x) && int.TryParse(coords[1], out int y))
                {
                    try
                    {
                        // Vérifier si le mouvement est valide avant de le faire
                        var targetPosition = new Position(x, y);
                        bool isValidMove = false;
                        
                        // Vérifier si la position est dans les positions valides
                        foreach (var pos in possiblePositions)
                        {
                            if (pos.GetPositionX() == x && pos.GetPositionY() == y)
                            {
                                isValidMove = true;
                                break;
                            }
                        }

                        if (isValidMove)
                        {
                            // Vérifier si le pion ne reste pas sur la même position
                            if (x == currentPosition.GetPositionX() && y == currentPosition.GetPositionY())
                            {
                                DisplayError("Vous ne pouvez pas rester sur la même position. Veuillez choisir une position valide.", playerColor);
                                return;
                            }

                            bool victory = currentRound.MovePawn(x, y);
                            
                            if (victory)
                            {
                                Console.ForegroundColor = currentPawn == board.Pawn1 ? ConsoleColor.Blue : ConsoleColor.Red;
                                Console.WriteLine($"\n=== Le joueur {(currentPawn == board.Pawn1 ? "1" : "2")} a gagné la manche ! ===");
                                Console.ResetColor();
                                Console.WriteLine($"Score actuel - Joueur 1: {gameManager.GetBestOf().GetPlayer1Score()}, Joueur 2: {gameManager.GetBestOf().GetPlayer2Score()}");
                                
                                if (gameManager.IsGameFinished())
                                {
                                    Console.ForegroundColor = currentPawn == board.Pawn1 ? ConsoleColor.Blue : ConsoleColor.Red;
                                    Console.WriteLine($"\n=== Le joueur {(currentPawn == board.Pawn1 ? "1" : "2")} a gagné la partie ! ===");
                                    Console.ResetColor();
                                    Console.WriteLine("\n=== Score final ===");
                                    Console.WriteLine($"Joueur 1: {gameManager.GetBestOf().GetPlayer1Score()}");
                                    Console.WriteLine($"Joueur 2: {gameManager.GetBestOf().GetPlayer2Score()}");
                                    Console.WriteLine("=====================");
                                    Console.WriteLine("\nAppuyez sur Entrée pour quitter...");
                                    Console.ReadLine();
                                    Environment.Exit(0);
                                }
                                else
                                {
                                    Console.WriteLine("\nAppuyez sur Entrée pour commencer une nouvelle manche...");
                                    Console.ReadLine();
                                    Console.WriteLine("\n=== Nouvelle manche ! ===");
                                    // Réinitialiser le jeu pour une nouvelle manche
                                    var players = gameManager.GetPlayers();
                                    gameManager.InitGame(players[0], players[1]);
                                    
                                    // Vérifier que le nouveau round existe avant d'afficher le plateau
                                    var newRound = gameManager.GetCurrentRound();
                                    if (newRound != null)
                                    {
                                        DisplayBoard(newRound.GetBoard());
                                    }
                                }
                            }
                            else
                            {
                                gameManager.PlayTurn();
                                DisplayBoard(currentRound.GetBoard());
                            }
                        }
                        else
                        {
                            DisplayError("Position invalide. Veuillez choisir une position valide parmi celles proposées.", playerColor);
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

        private static bool IsWallBlocking(Position from, Position to, Wall wall)
        {
            var wallStart = wall.GetFirstPosition();
            var wallEnd = wall.GetSecondPosition();
            
            // Vérifier si le mur est horizontal
            if (wallStart.GetPositionY() == wallEnd.GetPositionY())
            {
                // Vérifier si le mur bloque un mouvement vertical
                if (from.GetPositionX() == to.GetPositionX() && 
                    wallStart.GetPositionY() == Math.Min(from.GetPositionY(), to.GetPositionY()) + 1 &&
                    wallStart.GetPositionX() <= from.GetPositionX() && 
                    wallEnd.GetPositionX() >= from.GetPositionX())
                {
                    return true;
                }
            }
            // Vérifier si le mur est vertical
            else if (wallStart.GetPositionX() == wallEnd.GetPositionX())
            {
                // Vérifier si le mur bloque un mouvement horizontal
                if (from.GetPositionY() == to.GetPositionY() && 
                    wallStart.GetPositionX() == Math.Min(from.GetPositionX(), to.GetPositionX()) + 1 &&
                    wallStart.GetPositionY() <= from.GetPositionY() && 
                    wallEnd.GetPositionY() >= from.GetPositionY())
                {
                    return true;
                }
            }
            
            return false;
        }

        private static void HandlePlaceWall(Round currentRound, GameManager gameManager, ConsoleColor playerColor)
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
                        
                        // Vérifier si la position est valide pour un mur
                        if (!Board.IsWallONBoard(x, y, orientation))
                        {
                            DisplayError("Position invalide pour un mur. Les murs horizontaux doivent être entre 0-7 en x et 0-8 en y, les murs verticaux entre 0-8 en x et 0-7 en y.", playerColor);
                            return;
                        }

                        if (currentRound.PlacingWall(x, y, orientation))
                        {
                            gameManager.PlayTurn();
                            DisplayBoard(currentRound.GetBoard());
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