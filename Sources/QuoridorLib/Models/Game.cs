using System;
using System.Collections.Generic;

namespace QuoridorLib.Models
{
    public class Game
    {
        private readonly List<Player> players;
        private Round? currentRound;
        private readonly BestOf bestOf;

        public Game()
        {
            players = new List<Player>();
            bestOf = new BestOf(3);
            currentRound = null;
        }

        public void AddPlayer(Player player)
        {
            if (players.Count >= 2)
            {
                throw new InvalidOperationException("Cannot add more than 2 players");
            }
            players.Add(player);
        }

        public void LaunchRound()
        {
            if (players.Count != 2)
            {
                throw new InvalidOperationException("Besoin de 2 joueurs pour commencer une partie");
            }

            Board board = new Board();
            board.Init1vs1QuoridorBoard(
                players[0].Name,
                players[1].Name,
                new Position(4, 0),  // Player 1 starts at bottom
                new Position(4, 8)   // Player 2 starts at top
            );
            currentRound = new Round(players[0], board);
        }

        public Player? GetCurrentPlayer()
        {
            if (currentRound == null)
            {
                return null;
            }
            return currentRound.CurrentPlayer;
        }

        public List<Player> GetPlayers()
        {
            return players;
        }

        public BestOf GetBestOf()
        {
            return bestOf;
        }

        public bool IsGameOver()
        {
            return bestOf.GetPlayer1Score() >= bestOf.GetNumberOfGames() / 2 + 1 ||
                   bestOf.GetPlayer2Score() >= bestOf.GetNumberOfGames() / 2 + 1;
        }

        public Round? GetCurrentRound()
        {
            return currentRound;
        }
    }
} 