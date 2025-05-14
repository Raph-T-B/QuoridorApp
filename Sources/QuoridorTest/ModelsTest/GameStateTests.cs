using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;
using QuoridorLib.Models;
using QuoridorLib.Interfaces;

namespace QuoridorTest.ModelsTest
{
    public class GameStateTests
    {
        [Fact]
        public void Players_Property_Should_Be_ReadOnlyCollection()
        {
            // Arrange
            GameState gameState = new GameState();
            var players = new List<Player> { new Player("Player1"), new Player("Player2") };

            // Act
            gameState.Players = players.AsReadOnly();

            // Assert
            Assert.IsType<ReadOnlyCollection<Player>>(gameState.Players);
            Assert.Equal(2, gameState.Players.Count);
            Assert.Equal("Player1", gameState.Players[0].Name);
            Assert.Equal("Player2", gameState.Players[1].Name);
        }

        [Fact]
        public void Players_Property_Should_Not_Allow_Modification()
        {
            // Arrange
            GameState gameState = new GameState();
            var players = new List<Player> { new Player("Player1") };
            gameState.Players = players.AsReadOnly();

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => ((IList<Player>)gameState.Players).Add(new Player("Player2")));
            Assert.Throws<NotSupportedException>(() => ((IList<Player>)gameState.Players).RemoveAt(0));
            Assert.Throws<NotSupportedException>(() => ((IList<Player>)gameState.Players)[0] = new Player("Player2"));
        }
    }
} 