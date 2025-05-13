using System;
using System.Collections.Generic;

namespace QuoridorLib.Models
{
    public class Game
    {
        private List<Player> players;
        private BestOf bestOf;
        private Round? currentRound;
        private bool isGameOver;

        public Game()
        {
            players = new List<Player>();
            bestOf = new BestOf(3); // Default to best of 3 games
            isGameOver = false;
            currentRound = null;
        }

        public void AddPlayer(Player player)
        {
            if (players.Count < 2)
            {
                players.Add(player);
            }
            else
            {
                throw new InvalidOperationException("Cannot add more than 2 players to the game.");
            }
        }

        public void LaunchRound()
        {
            if (players.Count != 2)
            {
                throw new InvalidOperationException("Cannot start a round without exactly 2 players.");
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

        public void EndGame()
        {
            isGameOver = true;
        }

        public Player GetCurrentPlayer()
        {
            if (currentRound == null)
            {
                throw new InvalidOperationException("No round is currently active.");
            }
            return currentRound.CurrentPlayer;
        }

        public List<Player> GetPlayers()
        {
            return new List<Player>(players);
        }

        public BestOf GetBestOf()
        {
            return bestOf;
        }

        public bool IsGameOver()
        {
            return isGameOver;
        }

        public Round GetCurrentRound()
        {
            if (currentRound == null)
            {
                throw new InvalidOperationException("No round is currently active.");
            }
            return currentRound;
        }
    }
} 