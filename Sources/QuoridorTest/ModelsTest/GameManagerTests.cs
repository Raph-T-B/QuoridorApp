using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;
using QuoridorLib.Models;
using QuoridorLib.Interfaces;
using Moq;

namespace QuoridorTest.ModelsTest
{
    public class GameManagerTests
    {
        private readonly Mock<ILoadManager> _loadManagerMock;
        private readonly Mock<ISaveManager> _saveManagerMock;
        private readonly GameManager _gameManager;

        public GameManagerTests()
        {
            _loadManagerMock = new Mock<ILoadManager>();
            _saveManagerMock = new Mock<ISaveManager>();
            _gameManager = new GameManager(_loadManagerMock.Object, _saveManagerMock.Object);
        }

        [Fact]
        public void InitGame_Should_Initialize_Game_And_Raise_Events()
        {
            // Arrange
            var player1 = new Player("Player1");
            var player2 = new Player("Player2");
            bool gameInitializedRaised = false;
            bool gameStateChangedRaised = false;

            _gameManager.GameInitialized += (s, e) => 
            {
                gameInitializedRaised = true;
                Assert.Equal(player1, e.player1);
                Assert.Equal(player2, e.player2);
            };

            _gameManager.GameStateChanged += (s, e) => 
            {
                gameStateChangedRaised = true;
                Assert.NotNull(e.CurrentRound);
                Assert.Equal(2, e.Players.Count);
                Assert.NotNull(e.BestOf);
            };

            // Act
            _gameManager.InitGame(player1, player2);

            // Assert
            Assert.True(gameInitializedRaised);
            Assert.True(gameStateChangedRaised);
            Assert.Equal(player1, _gameManager.GetCurrentPlayer());
        }

        [Fact]
        public void PlayTurn_Should_Switch_Players_And_Raise_Events()
        {
            // Arrange
            var player1 = new Player("Player1");
            var player2 = new Player("Player2");
            _gameManager.InitGame(player1, player2);

            bool turnStartedRaised = false;
            bool turnEndedRaised = false;
            bool gameStateChangedRaised = false;

            _gameManager.TurnStarted += (s, e) => 
            {
                turnStartedRaised = true;
                Assert.Equal(player1, e);
            };

            _gameManager.TurnEnded += (s, e) => 
            {
                turnEndedRaised = true;
                Assert.Equal(player2, e);
            };

            _gameManager.GameStateChanged += (s, e) => 
            {
                gameStateChangedRaised = true;
                Assert.NotNull(e.CurrentRound);
                Assert.Equal(2, e.Players.Count);
            };

            // Act
            _gameManager.PlayTurn();

            // Assert
            Assert.True(turnStartedRaised);
            Assert.True(turnEndedRaised);
            Assert.True(gameStateChangedRaised);
            Assert.Equal(player2, _gameManager.GetCurrentPlayer());
        }

        [Fact]
        public void PlayTurn_Should_Not_Play_If_Game_Is_Over()
        {
            // Arrange
            var player1 = new Player("Player1");
            var player2 = new Player("Player2");
            _gameManager.InitGame(player1, player2);
            var bestOf = _gameManager.GetBestOf();
            bestOf.AddPlayer1Victory();
            bestOf.AddPlayer1Victory();

            bool turnStartedRaised = false;
            _gameManager.TurnStarted += (s, e) => turnStartedRaised = true;

            // Act
            _gameManager.PlayTurn();

            // Assert
            Assert.False(turnStartedRaised);
        }

        [Fact]
        public void GetPlayers_Should_Return_ReadOnlyCollection()
        {
            // Arrange
            var player1 = new Player("Player1");
            var player2 = new Player("Player2");
            _gameManager.InitGame(player1, player2);

            // Act
            var players = _gameManager.GetPlayers();

            // Assert
            Assert.IsType<ReadOnlyCollection<Player>>(players);
            Assert.Equal(2, players.Count);
            Assert.Equal("Player1", players[0].Name);
            Assert.Equal("Player2", players[1].Name);
        }

        
        

        [Fact]
        public void IsGameFinished_Should_Return_True_When_Game_Is_Over()
        {
            // Arrange
            var player1 = new Player("Player1");
            var player2 = new Player("Player2");
            _gameManager.InitGame(player1, player2);
            var bestOf = _gameManager.GetBestOf();
            bestOf.AddPlayer1Victory();
            bestOf.AddPlayer1Victory();

            bool gameFinishedRaised = false;
            _gameManager.GameFinished += (s, e) => gameFinishedRaised = true;

            // Act
            var isFinished = _gameManager.IsGameFinished();

            // Assert
            Assert.True(isFinished);
            Assert.True(gameFinishedRaised);
        }

        [Fact]
        public void LoadGame_Should_Return_Game_From_LoadManager()
        {
            // Arrange
            Game expectedGame = new();
            _loadManagerMock.Setup(x => x.LoadGame(0)).Returns(expectedGame);

            // Act
            var game = _gameManager.LoadGame();

            // Assert
            Assert.Equal(expectedGame, game);
            _loadManagerMock.Verify(x => x.LoadGame(0), Times.Once);
        }

        [Fact]
        public void SaveGame_Should_Save_Current_Game()
        {
            // Arrange
            var player1 = new Player("Player1");
            var player2 = new Player("Player2");
            _gameManager.InitGame(player1, player2);

            // Act
            _gameManager.SaveGame();

            // Assert
            _saveManagerMock.Verify(x => x.SaveGame(It.IsAny<Game>()), Times.Once);
        }
    }
} 