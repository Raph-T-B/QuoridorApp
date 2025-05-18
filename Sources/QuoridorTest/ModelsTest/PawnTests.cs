using Xunit;
using QuoridorLib.Models;
namespace QuoridorTest.ModelsTest;

public class PawnTests
{
    [Fact]
    public void Constructor_WithCoordinates_InitializesCorrectPosition()
    {
        Pawn pawn = new (3, 4);

        Assert.Equal(3, pawn.GetPositionX());
        Assert.Equal(4, pawn.GetPositionY());
        Assert.Equal(new Position(3, 4), pawn.GetPosition());
    }

    [Fact]
    public void Constructor_WithPosition_InitializesCorrectly()
    {
        Position position = new (5, 6);
        Pawn pawn = new (position);

        Assert.Equal(position, pawn.GetPosition());
        Assert.Equal(5, pawn.GetPositionX());
        Assert.Equal(6, pawn.GetPositionY());
    }

    [Fact]
    public void Move_ChangesPawnPosition()
    {
        Pawn pawn = new (0, 0);
        Position newPosition = new (7, 8);

        pawn.Move(newPosition);

        Assert.Equal(7, pawn.GetPositionX());
        Assert.Equal(8, pawn.GetPositionY());
        Assert.Equal(newPosition, pawn.GetPawnPosition());
    }

    [Fact]
    public void GetPawnPosition_ReturnsCurrentPosition()
    {
        Position position = new (2, 2);
        Pawn pawn = new (position);

        Position currentPosition = pawn.GetPawnPosition();

        Assert.Equal(position, currentPosition);
    }

    [Fact]
    public void SetPlayer_AssociatesPlayerCorrectly()
    {
        Pawn pawn = new (0, 0);
        Player player = new ("Alice");

        pawn.SetPlayer(player);

        Assert.Equal(player, pawn.GetPlayer());
    }

    [Fact]
    public void GetPlayer_WhenNotSet_ReturnsNull()
    {
        Pawn pawn = new (0, 0);

        Player? player = pawn.GetPlayer();

        Assert.Null(player);
    }
}
