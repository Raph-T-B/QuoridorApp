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



    [Theory]
    [InlineData( 1 , 5 )]
    [InlineData( 0 , 6 )]
    [InlineData( 0 , 4 )]
    public void MovePawn_ShouldMoveToValidAdjacentPosition(int x,int y)
    {
        // Arrange
        Board board = new ();
        Player player1 = new ("Alice");
        Player player2 = new ("Bob");
        board.Init1vs1QuoridorBoard(player1, player2);
        Position NextPosition = new(x, y);

        // Act
        bool movedP1 = board.MovePawn(board.Pawn1, NextPosition);

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

        board.MovePawn(board.Pawn1, new Position(1, 5));
        board.MovePawn(board.Pawn1, new Position(2, 5));
        board.MovePawn(board.Pawn1, new Position(3, 5));
        board.MovePawn(board.Pawn1, new Position(4, 5));
        board.MovePawn(board.Pawn1, new Position(5, 5));
        board.MovePawn(board.Pawn1, new Position(6, 5));
        Position lastPosition = new (7, 5);
        board.MovePawn(board.Pawn1, lastPosition);

        Position occupiedPosition = new(8, 5);

        // Act
        bool moved = board.MovePawn(board.Pawn1, occupiedPosition);

        // Assert
        Assert.False(moved);
        Assert.Equal(lastPosition, board.Pawn1.GetPawnPosition());
    }
     
    [Fact]
    public void AddCoupleWall_ShouldAddIfValid()
    {
        // Arrange
        Board board = new ();
        Wall wall1 = new (new Position(1, 1), new Position(1, 2));
        Wall wall2 = new (new Position(2, 1), new Position(2, 2));

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
        Board board = new ();
        Wall wall1 = new (new Position(1, 1), new Position(1, 2));
        Wall wall2 = new (new Position(2, 1), new Position(2, 2));
        board.AddCoupleWall(wall1, wall2, "vertical");

        Wall wall3 = new (new Position(1, 1), new Position(1, 2));
        Wall wall4 = new (new Position(2, 1), new Position(2, 2));

        // Act
        bool added = board.AddCoupleWall(wall3, wall4, "vertical");

        // Assert
        Assert.False(added);
        Assert.Single(board.WallCouples);
    }
    [Theory]
    [InlineData(-1, 5)] // Hors plateau
    [InlineData(0, 5)]  // Même position
    [InlineData(0, 7)]  // Non adjacent
    public void MovePawn_ShouldFailForInvalidPositions(int x, int y)
    {
        // Arrange
        Board board = new ();
        Player p1 = new ("Alice");
        Player p2 = new ("Bob");
        board.Init1vs1QuoridorBoard(p1, p2);

        Position nextPos = new (x, y);
        Position initPos = new(0, 5);

        // Act
        bool result = board.MovePawn(board.Pawn1, nextPos);

        // Assert
        Assert.False(result);
        Assert.Equal(initPos , board.Pawn1.GetPawnPosition());
    }

    [Fact]
    public void MovePawn_ShouldFailIfWallIsBlocking()
    {
        // Arrange
        Board board = new ();
        Player p1 = new ("Alice");
        Player p2 = new ("Bob");
        board.Init1vs1QuoridorBoard(p1, p2);

        // Act
        Wall wall1 = new (new Position(0, 5), new Position(1, 5));
        Wall wall2 = new (new Position(0, 6), new Position(1, 6));
        board.AddCoupleWall(wall1, wall2, "vertical");

        bool result = board.MovePawn(board.Pawn1, new Position(1, 5));

        // Assert
        Assert.False(result);
        Assert.Equal(new Position(0, 5), board.Pawn1.GetPawnPosition());
    }

    [Fact]
    public void AddCoupleWall_ShouldNotAddIfCrossing()
    {
        var board = new Board();
        var wall1 = new Wall(new Position(1, 1), new Position(1, 2)); // vertical
        var wall2 = new Wall(new Position(2, 1), new Position(2, 2));
        board.AddCoupleWall(wall1, wall2, "vertical");

        var crossWall1 = new Wall(new Position(0, 2), new Position(2, 2)); // horizontal croise
        var crossWall2 = new Wall(new Position(0, 3), new Position(2, 3));

        var result = board.AddCoupleWall(crossWall1, crossWall2, "horizontal");

        Assert.False(result);
        Assert.Single(board.WallCouples);
    }



}
