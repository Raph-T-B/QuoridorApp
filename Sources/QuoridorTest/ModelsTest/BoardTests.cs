using Xunit;
using QuoridorLib.Models;
using System.Linq;
using System.Collections.Generic;

namespace QuoridorTest.ModelsTest;

public class BoardTests
{
    [Fact]
    public void Init1vs1QuoridorBoard_ShouldInitializePawns()
    {
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");

        board.Init1vs1QuoridorBoard(player1, player2);

        Assert.Equal(new Position(0, 4), board.Pawn1.GetPawnPosition());
        Assert.Equal(new Position(8, 4), board.Pawn2.GetPawnPosition());
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(0, 5)]
    [InlineData(0, 3)]
    public void MovePawn_ShouldMoveToValidAdjacentPosition(int x, int y)
    {
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        board.Init1vs1QuoridorBoard(player1, player2);
        Position nextPosition = new(x, y);

        bool moved = board.MovePawn(board.Pawn1, nextPosition);

        Assert.True(moved);
        Assert.Equal(nextPosition, board.Pawn1.GetPawnPosition());
    }

    [Theory]
    [InlineData(6,5)]
    [InlineData(7,4)]
    [InlineData(6,3)]
    public void MovePawn_ShouldMoveToValidJumpedPosition(int x, int y)
    {
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        board.Init1vs1QuoridorBoard(player1, player2);
        board.MovePawn(board.Pawn1, new(1, 4));
        board.MovePawn(board.Pawn1, new(2, 4));
        board.MovePawn(board.Pawn1, new(3, 4));
        board.MovePawn(board.Pawn1, new(4, 4));
        board.MovePawn(board.Pawn1, new(5, 4));
        board.MovePawn(board.Pawn2, new(7, 4));
        board.MovePawn(board.Pawn2, new(6, 4));
        Position nextPosition = new(x, y);


        bool moved = board.MovePawn(board.Pawn1, nextPosition);

        Assert.True(moved);
        Assert.Equal(nextPosition, board.Pawn1.GetPawnPosition());
    }


   
    [Fact]
    public void MovePawn_ShouldFailIfTargetOccupied()
    {
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        board.Init1vs1QuoridorBoard(player1, player2);

        board.MovePawn(board.Pawn1, new Position(1, 4));
        board.MovePawn(board.Pawn1, new Position(2, 4));
        board.MovePawn(board.Pawn1, new Position(3, 4));
        board.MovePawn(board.Pawn1, new Position(4, 4));
        board.MovePawn(board.Pawn1, new Position(5, 4));
        board.MovePawn(board.Pawn1, new Position(6, 4));
        Position lastPosition = new(7, 4);
        board.MovePawn(board.Pawn1, lastPosition);

        Position occupiedPosition = new(8, 4);
        bool moved = board.MovePawn(board.Pawn1, occupiedPosition);

        Assert.False(moved);
        Assert.Equal(lastPosition, board.Pawn1.GetPawnPosition());
    }

    [Theory]
    [InlineData(-1, 4)]
    [InlineData(0, 4)]
    [InlineData(0, 7)]
    public void MovePawn_ShouldFailForInvalidPositions(int x, int y)
    {
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        board.Init1vs1QuoridorBoard(player1, player2);

        Position nextPos = new(x, y);
        Position initPos = new(0, 4);
        bool result = board.MovePawn(board.Pawn1, nextPos);

        Assert.False(result);
        Assert.Equal(initPos, board.Pawn1.GetPawnPosition());
    }

    [Fact]
    public void MovePawn_ShouldFailIfWallIsBlocking()
    {
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        board.Init1vs1QuoridorBoard(player1, player2);

        Wall wall1 = new(new Position(0, 4), new Position(1, 4));
        Wall wall2 = new(new Position(0, 5), new Position(1, 5));
        board.AddCoupleWall(wall1, wall2, "vertical");

        bool result = board.MovePawn(board.Pawn1, new Position(1, 4));

        Assert.False(result);
        Assert.Equal(new Position(0, 4), board.Pawn1.GetPawnPosition());
    }

    [Fact]
    public void AddCoupleWall_ShouldAddIfValid()
    {
        Board board = new();
        Wall wall1 = new(new Position(1, 1), new Position(1, 2));
        Wall wall2 = new(new Position(2, 1), new Position(2, 2));

        bool added = board.AddCoupleWall(wall1, wall2, "vertical");

        Assert.True(added);
        Assert.Single(board.WallCouples);
        Assert.Equal(wall1, board.WallCouples.First().GetWall1());
    }

    [Fact]
    public void AddCoupleWall_ShouldNotAddIfOverlapping()
    {
        Board board = new();
        Wall wall1 = new(new Position(1, 1), new Position(1, 2));
        Wall wall2 = new(new Position(2, 1), new Position(2, 2));
        board.AddCoupleWall(wall1, wall2, "vertical");

        Wall wall3 = new(new Position(1, 1), new Position(1, 2));
        Wall wall4 = new(new Position(2, 1), new Position(2, 2));

        bool added = board.AddCoupleWall(wall3, wall4, "vertical");

        Assert.False(added);
        Assert.Single(board.WallCouples);
    }

    [Fact]
    public void AddCoupleWall_ShouldNotAddIfCrossing()
    {
        Board board = new();
        Wall wall1 = new(new Position(1, 1), new Position(1, 2)); // vertical
        Wall wall2 = new(new Position(2, 1), new Position(2, 2));
        board.AddCoupleWall(wall1, wall2, "vertical");

        Wall crossWall1 = new(new Position(0, 2), new Position(2, 2)); // horizontal croise
        Wall crossWall2 = new(new Position(0, 3), new Position(2, 3));

        bool result = board.AddCoupleWall(crossWall1, crossWall2, "horizontal");

        Assert.False(result);
        Assert.Single(board.WallCouples);
    }

    [Fact]
    public void AddCoupleWall_ShouldReturnFalseWithInvalidOrientation()
    {
        Board board = new();
        Wall wall1 = new(new Position(1, 1), new Position(1, 2));
        Wall wall2 = new(new Position(2, 1), new Position(2, 2));

        bool result = board.AddCoupleWall(wall1, wall2, "diagonal");

        Assert.False(result);
        Assert.Empty(board.WallCouples);
    }

    [Theory]
    [InlineData(8, 7, "vertical", true)]
    [InlineData(9, 7, "vertical", false)]
    [InlineData(7, 8, "horizontal", true)]
    [InlineData(7, 9, "horizontal", false)]
    [InlineData(-1, 5, "vertical", false)]
    [InlineData(0, -1, "horizontal", false)]
    public void IsWallONBoard_ShouldReturnExpectedResult(int x, int y, string orientation, bool expected)
    {
        bool result = Board.IsWallONBoard(x, y, orientation);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void AreWallsCrossing_ShouldReturnTrueWhenWallsCross()
    {
        Wall verticalWall = new(new Position(1, 1), new Position(1, 2));
        Wall horizontalWall = new(new Position(0, 2), new Position(2, 2));

        bool result = Board.AreWallsCrossing(verticalWall, horizontalWall);

        Assert.True(result);
    }

    [Fact]
    public void AreWallsCrossing_ShouldReturnFalseWhenWallsDoNotCross()
    {
        Wall wall1 = new(new Position(1, 1), new Position(1, 2));
        Wall wall2 = new(new Position(2, 2), new Position(3, 2));

        bool result = Board.AreWallsCrossing(wall1, wall2);

        Assert.False(result);
    }

    [Fact]
    public void GetPawnsPositions_ShouldReturnCorrectPositions()
    {
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        board.Init1vs1QuoridorBoard(player1, player2);

        var positions = board.GetPawnsPositions();

        Assert.Equal(new Position(0, 4), positions[player1]);
        Assert.Equal(new Position(8, 4), positions[player2]);
    }

    [Fact]
    public void GetWallsPositions_ShouldReturnCorrectPositions()
    {
        Board board = new();
        Wall wall1 = new(new Position(1, 1), new Position(1, 2));
        Wall wall2 = new(new Position(2, 1), new Position(2, 2));
        board.AddCoupleWall(wall1, wall2, "vertical");

        var positions = board.GetWallsPositions();

        Assert.Equal(2, positions.Count);
        Assert.Contains((wall1.GetFirstPosition(), wall1.GetSecondPosition()), positions);
        Assert.Contains((wall2.GetFirstPosition(), wall2.GetSecondPosition()), positions);
    }

    [Fact]
    public void GetPossibleMoves_ShouldReturnValidMoves()
    {
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        board.Init1vs1QuoridorBoard(player1, player2);

        var moves = board.GetPossibleMoves(board.Pawn1);

        Assert.Contains(new Position(1, 4), moves);
        Assert.Contains(new Position(0, 3), moves);
        Assert.Contains(new Position(0, 5), moves);
    }


    [Fact]
    public void GetPossibleMoves_ShouldreturnValidMovesWhenPawnCanJump()
    {
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        board.Init1vs1QuoridorBoard(player1, player2);
        board.MovePawn(board.Pawn1, new(1, 4));
        board.MovePawn(board.Pawn1, new(2, 4));
        board.MovePawn(board.Pawn1, new(3, 4));
        board.MovePawn(board.Pawn1, new(4, 4));
        board.MovePawn(board.Pawn1, new(5, 4));
        board.MovePawn(board.Pawn2, new(7, 4));
        board.MovePawn(board.Pawn2, new(6, 4));

        var moves = board.GetPossibleMoves(board.Pawn1);

        Assert.Contains(new Position(4, 4), moves);
        Assert.Contains(new Position(5, 5), moves);
        Assert.Contains(new Position(5, 3), moves);
        Assert.Contains(new Position(6, 5), moves);
        Assert.Contains(new Position(6, 3), moves);
        Assert.Contains(new Position(7, 4), moves);
    }

    [Fact]
    public void GetPossibleMoves_ShouldNotReturnBlockedMoves()
    {
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        board.Init1vs1QuoridorBoard(player1, player2);

        Wall wall1 = new(new Position(0, 4), new Position(1, 4));
        Wall wall2 = new(new Position(0, 5), new Position(1, 5));
        board.AddCoupleWall(wall1, wall2, "vertical");

        var moves = board.GetPossibleMoves(board.Pawn1);

        Assert.DoesNotContain(new Position(1, 4), moves);
    }

    [Fact]
    public void GetPossibleMoves_ShouldNotReturnMovesOutsideBoard()
    {
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        board.Init1vs1QuoridorBoard(player1, player2);

        var moves = board.GetPossibleMoves(board.Pawn1);

        Assert.DoesNotContain(new Position(-1, 4), moves);
        Assert.DoesNotContain(new Position(0, -1), moves);
        Assert.DoesNotContain(new Position(9, 4), moves);
        Assert.DoesNotContain(new Position(0, 9), moves);
    }

    [Fact]
    public void GetPossibleMoves_ShouldNotReturnMovesToOccupiedPositions()
    {
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        board.Init1vs1QuoridorBoard(player1, player2);

        board.MovePawn(board.Pawn1, new Position(7, 4));
        var moves = board.GetPossibleMoves(board.Pawn2);

        Assert.DoesNotContain(new Position(8, 4), moves);
    }

    [Fact]
    public void AddCoupleWall_ShouldNotAddWallOutsideBoard()
    {
        Board board = new();
        Wall wall1 = new(new Position(-1, 1), new Position(-1, 2));
        Wall wall2 = new(new Position(0, 1), new Position(0, 2));

        bool result = board.AddCoupleWall(wall1, wall2, "vertical");

        Assert.False(result);
        Assert.Empty(board.WallCouples);
    }

    [Fact]
    public void AddCoupleWall_ShouldNotAddWallWithInvalidOrientation()
    {
        Board board = new();
        Wall wall1 = new(new Position(1, 1), new Position(1, 2));
        Wall wall2 = new(new Position(2, 1), new Position(2, 2));

        bool result = board.AddCoupleWall(wall1, wall2, "invalid");

        Assert.False(result);
        Assert.Empty(board.WallCouples);
    }

    [Fact]
    public void MovePawn_ShouldNotMoveToSamePosition()
    {
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        board.Init1vs1QuoridorBoard(player1, player2);

        bool result = board.MovePawn(board.Pawn1, new Position(0, 4));

        Assert.False(result);
        Assert.Equal(new Position(0, 4), board.Pawn1.GetPawnPosition());
    }

    [Fact]
    public void MovePawn_ShouldNotMoveToNoAdjacentPosition()
    {
        Board board = new();
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        board.Init1vs1QuoridorBoard(player1, player2);

        bool result = board.MovePawn(board.Pawn1, new Position(2, 4));

        Assert.False(result);
        Assert.Equal(new Position(0, 4), board.Pawn1.GetPawnPosition());
    }

    [Fact]
    public void GetWalls_WithNoWalls_ReturnsEmptyList()
    {
        // Arrange
        Board board = new();
        Player player1 = new("Player1");
        Player player2 = new("Player2");
        board.Init1vs1QuoridorBoard(player1, player2);

        // Act
        List<Wall> walls = board.GetWalls();

        // Assert
        Assert.Empty(walls);
    }

    [Fact]
    public void GetWalls_WithPlacedWalls_ReturnsAllWalls()
    {
        // Arrange
        Board board = new();
        Player player1 = new("Player1");
        Player player2 = new("Player2");
        board.Init1vs1QuoridorBoard(player1, player2);

        Wall wall1 = new(1, 1, 1, 2);
        Wall wall2 = new(2, 1, 2, 2);
        board.AddCoupleWall(wall1, wall2, "vertical");

        // Act
        List<Wall> walls = board.GetWalls();

        // Assert
        Assert.Equal(2, walls.Count);
        Assert.Contains(wall1, walls);
        Assert.Contains(wall2, walls);
    }

    [Fact]
    public void IsWinner_WithPawn1AtEnd_ReturnsTrue()
    {
        // Arrange
        Board board = new();
        Player player1 = new("Player1");
        Player player2 = new("Player2");
        board.Init1vs1QuoridorBoard(player1, player2);

        // Act
        bool result = board.IsWinner(board.Pawn1);

        // Assert
        Assert.False(result);

        // Move Pawn1 to winning position step by step
        board.MovePawn(board.Pawn1, new Position(1, 4));
        board.MovePawn(board.Pawn1, new Position(2, 4));
        board.MovePawn(board.Pawn1, new Position(3, 4));
        board.MovePawn(board.Pawn1, new Position(4, 4));
        board.MovePawn(board.Pawn1, new Position(5, 4));
        board.MovePawn(board.Pawn1, new Position(6, 4));
        board.MovePawn(board.Pawn1, new Position(7, 4));
        board.MovePawn(board.Pawn1, new Position(7, 3));
        board.MovePawn(board.Pawn1, new Position(8, 3));
        result = board.IsWinner(board.Pawn1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsWinner_WithPawn2AtEnd_ReturnsTrue()
    {
        // Arrange
        Board board = new();
        Player player1 = new("Player1");
        Player player2 = new("Player2");
        board.Init1vs1QuoridorBoard(player1, player2);

        // Act
        bool result = board.IsWinner(board.Pawn2);

        // Assert
        Assert.False(result);

        // Move Pawn2 to winning position step by step
        board.MovePawn(board.Pawn2, new Position(7, 4));
        board.MovePawn(board.Pawn2, new Position(6, 4));
        board.MovePawn(board.Pawn2, new Position(5, 4));
        board.MovePawn(board.Pawn2, new Position(4, 4));
        board.MovePawn(board.Pawn2, new Position(3, 4));
        board.MovePawn(board.Pawn2, new Position(2, 4));
        board.MovePawn(board.Pawn2, new Position(1, 4));
        board.MovePawn(board.Pawn2, new Position(1, 5));
        board.MovePawn(board.Pawn2, new Position(0, 5));
        result = board.IsWinner(board.Pawn2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsWinner_WithPawnsNotAtEnd_ReturnsFalse()
    {
        // Arrange
        Board board = new();
        Player player1 = new("Player1");
        Player player2 = new("Player2");
        board.Init1vs1QuoridorBoard(player1, player2);

        // Act
        bool result1 = board.IsWinner(board.Pawn1);
        bool result2 = board.IsWinner(board.Pawn2);

        // Assert
        Assert.False(result1);
        Assert.False(result2);
    }

    [Fact]
    public void IsWinner_WithPawnAtEnd_ReturnsTrue()
    {
        // Arrange
        Board board = new();
        Player player1 = new("Player1");
        Player player2 = new("Player2");
        board.Init1vs1QuoridorBoard(player1, player2);

        // Act
        bool result = board.IsWinner(board.Pawn1);

        // Assert
        Assert.False(result);

        // Move Pawn1 to winning position step by step
        board.MovePawn(board.Pawn1, new Position(1, 4));
        board.MovePawn(board.Pawn1, new Position(2, 4));
        board.MovePawn(board.Pawn1, new Position(3, 4));
        board.MovePawn(board.Pawn1, new Position(4, 4));
        board.MovePawn(board.Pawn1, new Position(5, 4));
        board.MovePawn(board.Pawn1, new Position(6, 4));
        board.MovePawn(board.Pawn1, new Position(7, 4));
        board.MovePawn(board.Pawn1, new Position(7, 5));
        board.MovePawn(board.Pawn1, new Position(8, 5));
        result = board.IsWinner(board.Pawn1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsWinner_WithPawnAtEnd_ReturnsTrueForPlayer2()
    {
        // Arrange
        Board board = new();
        Player player1 = new("Player1");
        Player player2 = new("Player2");
        board.Init1vs1QuoridorBoard(player1, player2);

        // Act
        bool result = board.IsWinner(board.Pawn2);

        // Assert
        Assert.False(result);

        // Move Pawn2 to winning position step by step
        board.MovePawn(board.Pawn2, new Position(7, 4));
        board.MovePawn(board.Pawn2, new Position(6, 4));
        board.MovePawn(board.Pawn2, new Position(5, 4));
        board.MovePawn(board.Pawn2, new Position(4, 4));
        board.MovePawn(board.Pawn2, new Position(3, 4));
        board.MovePawn(board.Pawn2, new Position(2, 4));
        board.MovePawn(board.Pawn2, new Position(1, 4));
        board.MovePawn(board.Pawn2, new Position(1, 3));
        board.MovePawn(board.Pawn2, new Position(0, 3));
        result = board.IsWinner(board.Pawn2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsWinner_WithPawnsNotAtEnd_ReturnsFalseForBothPlayers()
    {
        // Arrange
        Board board = new();
        Player player1 = new("Player1");
        Player player2 = new("Player2");
        board.Init1vs1QuoridorBoard(player1, player2);

        // Act
        bool result1 = board.IsWinner(board.Pawn1);
        bool result2 = board.IsWinner(board.Pawn2);

        // Assert
        Assert.False(result1);
        Assert.False(result2);
    }

    [Fact]
    public void MovePawn_ShouldMoveWhenHorizontalWallIsNotAligned()
    {
        // Arrange
        Board board = new();
        Player player1 = new("Player1");
        Player player2 = new("Player2");
        board.Init1vs1QuoridorBoard(player1, player2);

        // Place a horizontal wall
        Wall wall1 = new(new Position(1, 2), new Position(2, 2));
        Wall wall2 = new(new Position(2, 2), new Position(3, 2));
        board.AddCoupleWall(wall1, wall2, "horizontal");

        // Move pawn to a position where the wall is not aligned with the movement
        board.MovePawn(board.Pawn1, new Position(1, 4));
        bool result = board.MovePawn(board.Pawn1, new Position(1, 5));

        // Assert
        Assert.True(result);
        Assert.Equal(new Position(1, 5), board.Pawn1.GetPawnPosition());
    }


    [Fact]
    public void MovePawn_ShouldNotMoveWhenHorizontalWallBlocksFromAbove()
    {
        // Arrange
        Board board = new();
        Player player1 = new("Player1");
        Player player2 = new("Player2");
        board.Init1vs1QuoridorBoard(player1, player2);

        // Move pawn to position above wall
        board.MovePawn(board.Pawn1, new Position(1, 4));
        board.MovePawn(board.Pawn1, new Position(2, 4));
        board.MovePawn(board.Pawn1, new Position(3, 4));
        board.MovePawn(board.Pawn1, new Position(3, 3));

        // Place a horizontal wall
        Wall wall1 = new(new Position(2, 2), new Position(3, 2));
        Wall wall2 = new(new Position(3, 2), new Position(4, 2));
        board.AddCoupleWall(wall1, wall2, "horizontal");

        // Try to move the pawn down through the wall
        bool result = board.MovePawn(board.Pawn1, new Position(3, 2));

        // Assert
        Assert.False(result);
        Assert.Equal(new Position(3, 3), board.Pawn1.GetPawnPosition());
    }

    [Fact]
    public void MovePawn_ShouldMoveHorizontallyWhenHorizontalWallIsNotInPath()
    {
        // Arrange
        Board board = new();
        Player player1 = new("Player1");
        Player player2 = new("Player2");
        board.Init1vs1QuoridorBoard(player1, player2);

        // Place a horizontal wall
        Wall wall1 = new(new Position(1, 2), new Position(2, 2));
        Wall wall2 = new(new Position(2, 2), new Position(3, 2));
        board.AddCoupleWall(wall1, wall2, "horizontal");

        // Try to move the pawn horizontally where there is no wall
        bool result = board.MovePawn(board.Pawn1, new Position(1, 4));

        // Assert
        Assert.True(result);
        Assert.Equal(new Position(1, 4), board.Pawn1.GetPawnPosition());
    }

    [Fact]
    public void MovePawn_ShouldNotMoveWhenHorizontalWallIsNotAligned()
    {
        // Arrange
        Board board = new();
        Player player1 = new("Player1");
        Player player2 = new("Player2");
        board.Init1vs1QuoridorBoard(player1, player2);

        // Place a horizontal wall
        Wall wall1 = new(new Position(1, 2), new Position(2, 2));
        Wall wall2 = new(new Position(2, 2), new Position(3, 2));
        board.AddCoupleWall(wall1, wall2, "horizontal");

        // Try to move the pawn vertically where the wall is not aligned
        bool result = board.MovePawn(board.Pawn1, new Position(5, 3));

        // Assert
        Assert.True(result);
        Assert.Equal(new Position(5, 3), board.Pawn1.GetPawnPosition());
    }

    [Fact]
    public void MovePawn_ShouldMoveWhenHorizontalWallIsNotInRange()
    {
        // Arrange
        Board board = new();
        Player player1 = new("Player1");
        Player player2 = new("Player2");
        board.Init1vs1QuoridorBoard(player1, player2);

        // Place a horizontal wall
        Wall wall1 = new(new Position(1, 2), new Position(1, 3));
        Wall wall2 = new(new Position(2, 2), new Position(2, 3));
        board.AddCoupleWall(wall1, wall2, "horizontal");

        // Move pawn far from the wall
        board.MovePawn(board.Pawn1, new Position(1, 5));
        board.MovePawn(board.Pawn1, new Position(2, 5));
        board.MovePawn(board.Pawn1, new Position(3, 5));
        board.MovePawn(board.Pawn1, new Position(4, 5));
        board.MovePawn(board.Pawn1, new Position(5, 5));
        board.MovePawn(board.Pawn1, new Position(5, 4));
        board.MovePawn(board.Pawn1, new Position(5, 3));
        board.MovePawn(board.Pawn1, new Position(5, 2));

        // Try to move the pawn vertically where the wall is not in range
        bool result = board.MovePawn(board.Pawn1, new Position(5, 3));

        // Assert
        Assert.True(result);
        Assert.Equal(new Position(5, 3), board.Pawn1.GetPawnPosition());
    }
}
