using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuoridorLib.Models;

namespace QuoridorMaui.Models
{
    internal class ListItemSave
    {
        private readonly Game _game;
        public int BoGlobal => _game.GetBestOf().GetNumberOfGames();
        public int BoP1 => _game.GetBestOf().GetPlayer1Score();
        public int BoP2 => _game.GetBestOf().GetPlayer2Score();
        public string Player1 => _game.GetPlayers()[0].Name;
        public string Player2 => _game.GetPlayers()[1].Name;

        public ListItemSave(Game game)
        {
            _game = game;
        }
        
    }
}
