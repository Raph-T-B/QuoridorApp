using System.Collections.ObjectModel;

namespace QuoridorMaui.Models
{
    public class GameBoard
    {
        public int NbRows { get; } = 9;
        public int NbColumns { get; } = 9;
        public ObservableCollection<string> FlatMatrix { get; }

        public GameBoard()
        {
            FlatMatrix = new ObservableCollection<string>();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Initialiser la matrice avec des cases vides
            for (int i = 0; i < NbRows * NbColumns; i++)
            {
                FlatMatrix.Add("");
            }

            // Ajouter des tests visuels pour vérifier que la matrice s'affiche correctement
            // Placer un "1" dans le coin supérieur gauche
            SetCell(0, 0, "1");
            // Placer un "2" dans le coin supérieur droit
            SetCell(0, NbColumns - 1, "2");
            // Placer un "3" dans le coin inférieur gauche
            SetCell(NbRows - 1, 0, "3");
            // Placer un "4" dans le coin inférieur droit
            SetCell(NbRows - 1, NbColumns - 1, "4");
            // Placer un "X" au centre
            SetCell(NbRows / 2, NbColumns / 2, "X");
        }

        public void SetCell(int row, int column, string value)
        {
            int index = row * NbColumns + column;
            if (index >= 0 && index < FlatMatrix.Count)
            {
                FlatMatrix[index] = value;
            }
        }

        public string GetCell(int row, int column)
        {
            int index = row * NbColumns + column;
            if (index >= 0 && index < FlatMatrix.Count)
            {
                return FlatMatrix[index];
            }
            return "";
        }
    }
} 