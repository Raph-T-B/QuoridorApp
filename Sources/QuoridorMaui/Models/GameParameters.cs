namespace QuoridorMaui.Models
{
    public class GameParameters
    {
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public string Player1Color { get; set; }
        public string Player2Color { get; set; }
        public int NumberOfWalls { get; set; }
        public bool IsBotGame { get; set; }
        public int BestOf { get; set; } // 1, 3 ou 5

        public GameParameters()
        {
            Player1Name = "";
            Player2Name = "";
            Player1Color = "Bleu";
            Player2Color = "Rouge";
            NumberOfWalls = 10;
            IsBotGame = false;
            BestOf = 1;
        }
    }
} 