using Xunit;
using QuoridorLib.Models;

namespace QuoridorTest.ModelsTest;

public class BoardTests
{
    [Fact]
    public void Init1vs1QuoridorBoard_ShouldInitializePawns()
    {
        // Arrange
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");

        // Act
        board.Init1vs1QuoridorBoard(player1, player2);

        // Assert
        Assert.Equal(new Position(0, 5), board.Pawn1.GetPawnPosition());
        Assert.Equal(new Position(8, 5), board.Pawn2.GetPawnPosition());
    }

    public static IEnumerable<object[]> ValidPositions =>
        new List<object[]>
        {
            new object[] { new Position(1, 5) },
            new object[] { new Position(0, 6) },
            new object[] { new Position(1, 4) },
        };

    [Theory]
    [InlineData( 1 , 5 )]
    [InlineData( 0 , 6 )]
    [InlineData( 0 , 4 )]
    public void MovePawn_ShouldMoveToValidAdjacentPosition(int x,int y)
    {
        // Arrange
        var board = new Board();
        var player1 = new Player("Alice");
        var player2 = new Player("Bob");
        board.Init1vs1QuoridorBoard(player1, player2);
        Position NextPosition = new(x, y);

        // Act
        bool movedP1 = board.MovePawn(player1, NextPosition);

        // Assert
        Assert.True(movedP1);
        Assert.Equal(NextPosition, board.Pawn1.GetPawnPosition());
    }

    [Fact]
    public void MovePawn_ShouldFailIfTargetOccupied()
    {
        // Arrange
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");

        board.Init1vs1QuoridorBoard(player1, player2);

        board.MovePawn(player1, new Position(1, 5));
        board.MovePawn(player1, new Position(2, 5));
        board.MovePawn(player1, new Position(3, 5));
        board.MovePawn(player1, new Position(4, 5));
        board.MovePawn(player1, new Position(5, 5));
        board.MovePawn(player1, new Position(6, 5));
        Position lastPosition = new (7, 5);
        board.MovePawn(player1, lastPosition);

        Position occupiedPosition = new(8, 5);

        // Act
        bool moved = board.MovePawn(player1, occupiedPosition);

        // Assert
        Assert.False(moved);
        Assert.Equal(lastPosition, board.Pawn1.GetPawnPosition());
    }
     
    [Fact]
    public void AddCoupleWall_ShouldAddIfValid()
    {
        // Arrange
        var board = new Board();
        var wall1 = new Wall(new Position(1, 1), new Position(1, 2));
        var wall2 = new Wall(new Position(2, 1), new Position(2, 2));

        // Act
        bool added = board.AddCoupleWall(wall1, wall2, "vertical");

        // Assert
        Assert.True(added);
        Assert.Single(board.WallCouples);
        Assert.Equal(board.WallCouples.First().GetWall1() ,wall1);
    }

    [Fact]
    public void AddCoupleWall_ShouldNotAddIfOverlapping()
    {
        // Arrange
        var board = new Board();
        var wall1 = new Wall(new Position(1, 1), new Position(1, 2));
        var wall2 = new Wall(new Position(2, 1), new Position(2, 2));
        board.AddCoupleWall(wall1, wall2, "vertical");

        // Attempt to add overlapping wall
        var wall3 = new Wall(new Position(1, 1), new Position(1, 2));
        var wall4 = new Wall(new Position(2, 1), new Position(2, 2));

        // Act
        bool added = board.AddCoupleWall(wall3, wall4, "vertical");

        // Assert
        Assert.False(added);
        Assert.Single(board.WallCouples);
    }
}
