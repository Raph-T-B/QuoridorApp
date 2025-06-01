using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;

namespace QuoridorMaui.Models
{
    public class GameBoard
    {
        public int NbRows { get; } = 9;
        public int NbColumns { get; } = 9;
        public ObservableCollection<CellContent> FlatMatrix { get; }

        public GameBoard(Color player1Color, Color player2Color)
        {
            FlatMatrix = new ObservableCollection<CellContent>();
            InitializeBoard(player1Color, player2Color);
        }

        private void InitializeBoard(Color player1Color, Color player2Color)
        {
            // Initialiser la matrice avec des cases vides
            for (int i = 0; i < NbRows * NbColumns; i++)
            {
                FlatMatrix.Add(new CellContent { Symbol = "", Color = null });
            }

            // Placer le joueur 1 à (0,4)
            SetCell(0, 4, "1", player1Color);
            // Placer le joueur 2 à (8,4)
            SetCell(8, 4, "2", player2Color);
        }

        public void SetCell(int x, int y, string value, Color? color = null)
        {
            int index = (NbRows - 1 - y) * NbColumns + x;
            if (index >= 0 && index < FlatMatrix.Count)
            {
                FlatMatrix[index] = new CellContent { Symbol = value, Color = color };
            }
        }

        public string GetCell(int x, int y)
        {
            int index = (NbRows - 1 - y) * NbColumns + x;
            if (index >= 0 && index < FlatMatrix.Count)
            {
                return FlatMatrix[index].Symbol;
            }
            return "";
        }
    }

    public class CellContent
    {
        public string Symbol { get; set; }
        public Color? Color { get; set; }
    }
} 