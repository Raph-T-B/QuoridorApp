using Microsoft.Maui.Graphics;

namespace QuoridorMaui.Models
{
    public class GameParameters
    {
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public Color Player1Color { get; set; }
        public Color Player2Color { get; set; }
        public int NumberOfWalls { get; set; }
        public bool IsBotGame { get; set; }
        public int BestOf { get; set; } // 1, 3 ou 5

        public GameParameters()
        {
            Player1Name = "";
            Player2Name = "";
            Player1Color = Colors.Blue;
            Player2Color = Colors.Red;
            NumberOfWalls = 10;
            IsBotGame = false;
            BestOf = 1;
        }
    }
} 