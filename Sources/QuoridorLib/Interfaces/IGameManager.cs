using QuoridorLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace QuoridorLib.Interfaces
{
    public interface IGameManager
    {
        event EventHandler<(Player player1, Player player2)> GameInitialized;
        event EventHandler<Player> TurnStarted;
        event EventHandler<Player> TurnEnded;
        event EventHandler<BestOf> GameFinished;
        event EventHandler<GameState> GameStateChanged;

        void InitGame(Player player1, Player player2, int numberOfGames = 3);
        void PlayTurn();
        bool IsGameFinished();
        public void LoadGame(Game game);
        public List<Game> LoadedGames();
        public List<Player> LoadedPlayers();
        public void SavePlayers(List<Player> PlayersAllreadySaved);
        public void SaveGames(List<Game> GamesAllreadySaved);
        public void SaveGamePlayers();
        void SaveGame();
        Round? GetCurrentRound();
        Player? GetCurrentPlayer();
        ReadOnlyCollection<Player> GetPlayers();
        BestOf GetBestOf();
    }
} 