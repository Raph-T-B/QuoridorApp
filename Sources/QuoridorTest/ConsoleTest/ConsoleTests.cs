using QuoridorLib.Models;
using QuoridorLib.Interfaces;
using System.Text;
using Xunit;

namespace QuoridorTest.ConsoleTest;

public class ConsoleTests
{
    private readonly StringWriter _output;
    private readonly StringReader _input;
    private readonly StringBuilder _inputBuilder;
    private readonly GameManager _gameManager;

    public ConsoleTests()
    {
        _output = new StringWriter();
        _inputBuilder = new StringBuilder();
        _input = new StringReader(_inputBuilder.ToString());
        Console.SetOut(_output);
        Console.SetIn(_input);

        ILoadManager loadManager = new StubLoadManager();
        ISaveManager saveManager = new StubSaveManager();
        _gameManager = new GameManager(loadManager, saveManager);
    }

    [Fact]
    public void TestPawnMovement()
    {
        // Arrange
        var player1 = new Player("Player1");
        var player2 = new Player("Player2");
        _gameManager.InitGame(player1, player2);

        var currentRound = _gameManager.GetCurrentRound();
        Assert.NotNull(currentRound);
        currentRound.SetGame(_gameManager.LoadGame());

        // Act
        bool success = currentRound.MovePawn(1, 4); // Mouvement vers la droite

        // Assert
        Assert.True(success);
        var board = currentRound.GetBoard();
        var pawns = board.GetPawnsPositions();
        Assert.Equal(1, pawns[player1].GetPositionX());
        Assert.Equal(4, pawns[player1].GetPositionY());
    }

    [Fact]
    public void TestInvalidPawnMovement()
    {
        // Arrange
        var player1 = new Player("Player1");
        var player2 = new Player("Player2");
        _gameManager.InitGame(player1, player2);

        var currentRound = _gameManager.GetCurrentRound();
        Assert.NotNull(currentRound);
        currentRound.SetGame(_gameManager.LoadGame());

        // Act
        bool success = currentRound.MovePawn(2, 4); // Mouvement de 2 cases vers la droite

        // Assert
        Assert.False(success);
        var board = currentRound.GetBoard();
        var pawns = board.GetPawnsPositions();
        Assert.Equal(0, pawns[player1].GetPositionX()); // Le pion n'a pas bougé
        Assert.Equal(4, pawns[player1].GetPositionY());
    }
/*
    [Fact]
    public void TestVictoryDetection()
    {
        // Arrange
        var player1 = new Player("Player1");
        var player2 = new Player("Player2");
        _gameManager.InitGame(player1, player2);

        var currentRound = _gameManager.GetCurrentRound();
        Assert.NotNull(currentRound);
        currentRound.SetGame(_gameManager.LoadGame());

        // Simuler une série de mouvements pour amener le joueur 1 à la victoire
        currentRound.MovePawn(1, 5);
        _gameManager.PlayTurn();
        currentRound = _gameManager.GetCurrentRound();
        Assert.NotNull(currentRound);
        currentRound.SetGame(_gameManager.LoadGame());

        currentRound.MovePawn(2, 5);
        _gameManager.PlayTurn();
        currentRound = _gameManager.GetCurrentRound();
        Assert.NotNull(currentRound);
        currentRound.SetGame(_gameManager.LoadGame());

        currentRound.MovePawn(3, 5);
        _gameManager.PlayTurn();
        currentRound = _gameManager.GetCurrentRound();
        Assert.NotNull(currentRound);
        currentRound.SetGame(_gameManager.LoadGame());

        currentRound.MovePawn(4, 5);
        _gameManager.PlayTurn();
        currentRound = _gameManager.GetCurrentRound();
        Assert.NotNull(currentRound);
        currentRound.SetGame(_gameManager.LoadGame());

        currentRound.MovePawn(5, 5);
        _gameManager.PlayTurn();
        currentRound = _gameManager.GetCurrentRound();
        Assert.NotNull(currentRound);
        currentRound.SetGame(_gameManager.LoadGame());

        currentRound.MovePawn(6, 5);
        _gameManager.PlayTurn();
        currentRound = _gameManager.GetCurrentRound();
        Assert.NotNull(currentRound);
        currentRound.SetGame(_gameManager.LoadGame());

        currentRound.MovePawn(7, 5);
        _gameManager.PlayTurn();
        currentRound = _gameManager.GetCurrentRound();
        Assert.NotNull(currentRound);
        currentRound.SetGame(_gameManager.LoadGame());

        bool victory = currentRound.MovePawn(8, 5); // Mouvement vers la ligne de victoire

        // Assert
        Assert.True(victory);
        var bestOf = _gameManager.GetBestOf();
        Assert.Equal(1, bestOf.GetPlayer1Score());
        Assert.Equal(0, bestOf.GetPlayer2Score());
    }*/

    [Fact]
    public void TestWallPlacement()
    {
        // Arrange
        var player1 = new Player("Player1");
        var player2 = new Player("Player2");
        _gameManager.InitGame(player1, player2);

        var currentRound = _gameManager.GetCurrentRound();
        Assert.NotNull(currentRound);
        currentRound.SetGame(_gameManager.LoadGame());

        // Act
        bool success = currentRound.PlacingWall(4, 4, "horizontal");

        // Assert
        Assert.True(success);
        var board = currentRound.GetBoard();
        var walls = board.GetWallsPositions();
        Assert.Equal(2, walls.Count); // Un couple de murs (2 murs) a été placé
    }

    [Fact]
    public void TestInvalidWallPlacement()
    {
        // Arrange
        var player1 = new Player("Player1");
        var player2 = new Player("Player2");
        _gameManager.InitGame(player1, player2);

        var currentRound = _gameManager.GetCurrentRound();
        Assert.NotNull(currentRound);
        currentRound.SetGame(_gameManager.LoadGame());

        // Placer un premier mur
        currentRound.PlacingWall(4, 4, "horizontal");

        // Act - Essayer de placer un mur qui se croise
        bool success = currentRound.PlacingWall(4, 4, "vertical");

        // Assert
        Assert.False(success);
        var board = currentRound.GetBoard();
        var walls = board.GetWallsPositions();
        Assert.Equal(2, walls.Count); // Un seul couple de murs (2 murs) a été placé
    }

    [Fact]
    public void TestGameFlow()
    {
        // Arrange
        var player1 = new Player("Player1");
        var player2 = new Player("Player2");
        _gameManager.InitGame(player1, player2);

        // Act & Assert
        // Premier tour - Joueur 1
        var currentRound = _gameManager.GetCurrentRound();
        Assert.NotNull(currentRound);
        currentRound.SetGame(_gameManager.LoadGame());
        Assert.Equal(player1, currentRound.CurrentPlayerProperty);

        // Déplacer le pion du joueur 1
        currentRound.MovePawn(1, 4);
        _gameManager.PlayTurn();

        // Deuxième tour - Joueur 2
        currentRound = _gameManager.GetCurrentRound();
        Assert.NotNull(currentRound);
        currentRound.SetGame(_gameManager.LoadGame());
        Assert.Equal(player2, currentRound.CurrentPlayerProperty);

        // Déplacer le pion du joueur 2
        currentRound.MovePawn(7, 4);
        _gameManager.PlayTurn();

        // Vérifier que le tour revient au joueur 1
        currentRound = _gameManager.GetCurrentRound();
        Assert.NotNull(currentRound);
        currentRound.SetGame(_gameManager.LoadGame());
        Assert.Equal(player1, currentRound.CurrentPlayerProperty);
    }

    private class StubLoadManager : ILoadManager
    {
        public Game LoadGame()
        {
            var game = new Game();
            game.AddPlayer(new Player("Player1"));
            game.AddPlayer(new Player("Player2"));
            game.LaunchRound();
            return game;
        }
        public GameState LoadGameState() => new GameState();
    }

    private class StubSaveManager : ISaveManager
    {
        public void SaveGame(Game game) { }
        public void SaveGameState(GameState gameState) { }
    }
}
