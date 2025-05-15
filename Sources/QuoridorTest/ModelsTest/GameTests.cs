using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;
using QuoridorLib.Models;

namespace QuoridorTest.ModelsTest
{
    public class GameTests
    {
        [Fact]
        public void GetPlayers_Should_Return_ReadOnlyCollection()
        {
            // Arrange
            Game game = new ();
            Player player1 = new ("Player1");
            Player player2 = new ("Player2");
            game.AddPlayer(player1);
            game.AddPlayer(player2);

            // Act
            var players = game.GetPlayers();

            // Assert
            Assert.IsType<ReadOnlyCollection<Player>>(players);
            Assert.Equal(2, players.Count);
            Assert.Equal("Player1", players[0].Name);
            Assert.Equal("Player2", players[1].Name);
        }

        [Fact]
        public void GetPlayers_Should_Not_Allow_Modification()
        {
            // Arrange
            Game game = new ();
            Player player1 = new ("Player1");
            game.AddPlayer(player1);

            // Act
            var players = game.GetPlayers();

            // Assert
            Assert.Throws<NotSupportedException>(() => ((IList<Player>)players).Add(new Player("Player2")));
            Assert.Throws<NotSupportedException>(() => ((IList<Player>)players).RemoveAt(0));
            Assert.Throws<NotSupportedException>(() => ((IList<Player>)players)[0] = new Player("Player2"));
        }
    }
} 