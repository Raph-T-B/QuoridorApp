using System;

namespace QuoridorLib.Models
{
    public class BestOf
    {
        private int player1Score;
        private int player2Score;
        private readonly int numberOfGames;

        public BestOf(int numberOfGames)
        {
            this.numberOfGames = numberOfGames;
            player1Score = 0;
            player2Score = 0;
        }

        public int GetPlayer1Score()
        {
            return player1Score;
        }

        public int GetPlayer2Score()
        {
            return player2Score;
        }

        public void AddPlayer1Victory()
        {
            player1Score++;
        }

        public void AddPlayer2Victory()
        {
            player2Score++;
        }


        public int GetNumberOfGames()
        {
            return numberOfGames;
        }

    }
} 