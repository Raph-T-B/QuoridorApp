using QuoridorLib.Models;

namespace QuoridorLib.Interfaces
{
    public interface IGameManager
    {
        void InitGame(Player player1, Player player2);
        Game LoadGame();
        void PlayTurn();
        bool IsGameFinished();
    }
} 