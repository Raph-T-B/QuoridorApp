using QuoridorLib.Models;
using System;

namespace QuoridorLib.Interfaces
{
    public interface IGameManager
    {
        event EventHandler<(Player player1, Player player2)> GameInitialized;
        event EventHandler<Player> TurnStarted;
        event EventHandler<Player> TurnEnded;
        event EventHandler<BestOf> GameFinished;
        event EventHandler<GameState> GameStateChanged;

        void InitGame(Player player1, Player player2);
        void PlayTurn();
        bool IsGameFinished();
        Game LoadGame();
        void SaveGame();
        Round GetCurrentRound();
        Player GetCurrentPlayer();
        List<Player> GetPlayers();
        BestOf GetBestOf();
        void SaveGameState();
        void LoadGameState();
    }
} 