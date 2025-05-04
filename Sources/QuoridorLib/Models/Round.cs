using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorLib.Models
{
    
   class Round : Board
    {

        // ✅ Propriété publique avec accès en lecture (get) mais écriture interne uniquement (private set)
        public Player CurrentPlayer { get; private set; }

        public Round(Game game)
        {
            _game = game;
            CurrentPlayer = game.GetFirstPlayer(); // On initialise le joueur courant
        }

        public void StartRound()
        {
            Console.WriteLine($"Début du tour pour : {CurrentPlayer.Name}");
        }

        public bool IsWinned()
        {
            return CurrentPlayer.HasWon();
        }

        public void SwitchPlayer()
        {
            CurrentPlayer = _game.GetNextPlayer(CurrentPlayer);
        }

        public void MovePawn(int newX, int newY)
        {
            CurrentPlayer.MovePawn(newX, newY);
        }

        public void PlacingWall(int x, int y, string orientation)
        {
            CurrentPlayer.PlaceWall(x, y, orientation);
        }
    }

}
}
