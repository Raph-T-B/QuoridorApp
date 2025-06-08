using Xunit;
using QuoridorLib.Models;

namespace QuoridorTest.ModelsTest;

public class RoundTests
{
    [Fact]
    public void SwitchCurrentPlayer_ShouldChangeCurrentPlayer()
    {
        // Arrange
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        Board board = new();
        board.Init1vs1QuoridorBoard(player1, player2);
        Round round = new(player1, board);

        // Act
        round.SwitchCurrentPlayer(player2);

        // Assert
        Assert.Equal(player2, round.CurrentPlayerProperty);
    }

    [Fact]
    public void MovePawn_ShouldMoveToValidPosition()
    {
        // Arrange
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        Board board = new();
        board.Init1vs1QuoridorBoard(player1, player2);
        Round round = new(player1, board);

        // Act
        round.MovePawn(1, 4);

        // Assert
        Assert.Equal(new Position(1, 4), board.Pawn1.GetPawnPosition());
    }

    [Fact]
    public void MovePawn_ShouldNotMoveToInvalidPosition()
    {
        // Arrange
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        Board board = new();
        board.Init1vs1QuoridorBoard(player1, player2);
        Round round = new(player1, board);

        // Act
        round.MovePawn(2, 4); // Position non adjacente

        // Assert
        Assert.Equal(new Position(0, 4), board.Pawn1.GetPawnPosition());
    }

    [Fact]
    public void PlacingWall_ShouldPlaceWallInValidPosition()
    {
        // Arrange
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        Board board = new();
        board.Init1vs1QuoridorBoard(player1, player2);
        Round round = new(player1, board);

        // Act
        bool result = round.PlacingWall(1, 1, "vertical");

        // Assert
        Assert.True(result);
        Assert.Single(board.WallCouples); // Vérifie qu'un couple de murs a été ajouté
    }

    [Fact]
    public void PlacingWall_ShouldNotPlaceWallInInvalidPosition()
    {
        // Arrange
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        Board board = new();
        board.Init1vs1QuoridorBoard(player1, player2);
        Round round = new(player1, board);

        // Act
        bool result = round.PlacingWall(9, 9, "vertical"); // Position hors plateau

        // Assert
        Assert.False(result);
        Assert.Empty(board.WallCouples);
    }

    [Fact]
    public void PlacingWall_ShouldNotPlaceWallIfBlockingPath()
    {
        // Arrange
        Player player1 = new("Alice");
        Player player2 = new("Bob");
        Board board = new();
        board.Init1vs1QuoridorBoard(player1, player2);
        Round round = new(player1, board);

        // Place un premier mur
        round.PlacingWall(1, 1, "vertical");

        // Act
        bool result = round.PlacingWall(1, 1, "vertical"); // Même position

        // Assert
        Assert.False(result);
        Assert.Single(board.WallCouples); // Vérifie qu'aucun couple de murs supplémentaire n'a été ajouté
    }
} 