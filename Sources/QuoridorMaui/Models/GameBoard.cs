#nullable enable
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

        public void InitializeBoard(Color player1Color, Color player2Color)
        {
            FlatMatrix.Clear();
            for (int i = 0; i < NbRows * NbColumns; i++)
            {
                FlatMatrix.Add(new CellContent { Symbol = "", Color = null, IsMovePossible = false });
            }
        }

        public void SetCell(int x, int y, string value, Color? color = null, bool isMovePossible = false)
        {
            int index = y * NbColumns + x;
            if (index >= 0 && index < FlatMatrix.Count)
            {
                var cell = new CellContent 
                { 
                    Symbol = value, 
                    Color = color, 
                    IsMovePossible = isMovePossible 
                };
                FlatMatrix[index] = cell;
            }
        }

        public string GetCell(int x, int y)
        {
            int index = y * NbColumns + x;
            if (index >= 0 && index < FlatMatrix.Count)
            {
                return FlatMatrix[index].Symbol;
            }
            return "";
        }

        public void UpdatePossibleMoves(IEnumerable<(int x, int y)> possibleMoves)
        {
            // Réinitialiser uniquement IsMovePossible sur les objets existants
            for (int i = 0; i < FlatMatrix.Count; i++)
            {
                if (FlatMatrix[i] != null)
                    FlatMatrix[i].IsMovePossible = false;
            }

            // Marquer les mouvements possibles
            foreach (var (x, y) in possibleMoves)
            {
                int index = y * NbColumns + x;
                if (index >= 0 && index < FlatMatrix.Count && FlatMatrix[index] != null)
                {
                    FlatMatrix[index].IsMovePossible = true;
                }
            }
        }

        /// <summary>
        /// Clears the content of a cell at the specified coordinates.
        /// </summary>
        /// <param name="x">The X coordinate of the cell</param>
        /// <param name="y">The Y coordinate of the cell</param>
        public void ClearCell(int x, int y)
        {
            SetCell(x, y, "", null, false);
        }
    }

    public class CellContent
    {
        public string Symbol { get; set; }
        public Color? Color { get; set; }
        public bool IsMovePossible { get; set; }
        public Color BackgroundColor => IsMovePossible ? Colors.LightGreen : Colors.White;
        public bool IsPawn => !string.IsNullOrEmpty(Symbol) && (Symbol == "1" || Symbol == "2");
    }
} 