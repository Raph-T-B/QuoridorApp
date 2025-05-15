using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuoridorLib.Models;

namespace QuoridorTest.ModelsTest;

public class WallCoupleTests
{
        [Fact]
        public void WallCouple_StoresWallsCorrectly()
        {
            // Arrange
            Position pos1 = new Position(1, 2);
            Position pos2 = new Position(1, 3);
            Position pos3 = new Position(2, 2);
            Position pos4 = new Position(2, 3);

            Wall wall1 = new Wall(pos1, pos2);
            Wall wall2 = new Wall(pos3, pos4);
            string orientation = "vertical";

            // Act
            WallCouple couple = new WallCouple(wall1, wall2, orientation);

            // Assert
            Assert.Equal(wall1, couple.GetWall1());
            Assert.Equal(wall2, couple.GetWall2());
            Assert.Equal(orientation, couple.GetOrientation());
        }

    [Theory]
    [InlineData("vertical")]
    [InlineData("horizontal")]
    public void WallCouple_AllowsGoodOrientation(string orient)
        {
            // Arrange
            Wall wall1 = new Wall(new Position(0, 0), new Position(1, 0));
            Wall wall2 = new Wall(new Position(1, 0), new Position(2, 0));
            string orientation = orient ;

            // Act
            WallCouple couple = new WallCouple(wall1, wall2, orientation);

            // Assert
            Assert.Equal(orient, couple.GetOrientation());
        }            
}

