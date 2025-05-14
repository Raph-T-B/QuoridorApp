using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;
using QuoridorLib.Models;

namespace QuoridorTest.ModelsTest
{
    public class BoardTests
    {
        [Fact]
        public void GetWalls_Should_Return_ReadOnlyCollection()
        {
            // Arrange
            Board board = new Board();
            Wall wall1 = new Wall(new Position(0, 0), new Position(0, 1));
            Wall wall2 = new Wall(new Position(1, 0), new Position(1, 1));
            board.AddCoupleWall(wall1, wall2);

            // Act
            var walls = board.GetWalls();

            // Assert
            Assert.IsType<ReadOnlyCollection<Wall>>(walls);
            Assert.Equal(2, walls.Count);
        }

        [Fact]
        public void GetWalls_Should_Not_Allow_Modification()
        {
            // Arrange
            Board board = new Board();
            Wall wall1 = new Wall(new Position(0, 0), new Position(0, 1));
            Wall wall2 = new Wall(new Position(1, 0), new Position(1, 1));
            board.AddCoupleWall(wall1, wall2);

            // Act
            var walls = board.GetWalls();

            // Assert
            Assert.Throws<NotSupportedException>(() => ((IList<Wall>)walls).Add(new Wall(new Position(2, 0), new Position(2, 1))));
            Assert.Throws<NotSupportedException>(() => ((IList<Wall>)walls).RemoveAt(0));
            Assert.Throws<NotSupportedException>(() => ((IList<Wall>)walls)[0] = new Wall(new Position(2, 0), new Position(2, 1)));
        }
    }
} 