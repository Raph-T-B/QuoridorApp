using System.Collections.ObjectModel;
using QuoridorLib.Interfaces;
using QuoridorLib.Models;

namespace QuoridorMaui.Persistence
{
    class PlayersPersistence : IGameManager
    {
        private string FileName { get; set; }



        public BestOf GetBestOf()
        {
            throw new NotImplementedException();
        }

        public Player? GetCurrentPlayer()
        {
            throw new NotImplementedException();
        }

        public Round? GetCurrentRound()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<Player> GetPlayers()
        {
            throw new NotImplementedException();
        }

        public void InitGame(Player player1, Player player2, int numberOfGames = 3)
        {
            throw new NotImplementedException();
        }

        public bool IsGameFinished()
        {
            throw new NotImplementedException();
        }

        public Game LoadGame()
        {
            throw new NotImplementedException();
        }

        public void LoadGameState()
        {
            throw new NotImplementedException();
        }

        public void PlayTurn()
        {
            throw new NotImplementedException();
        }

        public void SaveGame()
        {
            throw new NotImplementedException();
        }

        public void SaveGameState()
        {
            throw new NotImplementedException();
        }

        private void SavePlayers()
        {
            using Stream s = File.Create(FileName);
            
        }
    }
}
