#nullable enable

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using QuoridorLib.Models;
using QuoridorLib.Managers;
using QuoridorStub.Stub;

namespace QuoridorMaui.Models
{
    public class PlayingPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CellContent> FlatMatrix { get; }
        public ICommand CellClickedCommand { get; }
        public ICommand PlaceWallCommand { get; }
        public ICommand UndoCommand { get; }

        private string _player1Name;
        private string _player2Name;
        private Color _player1Color;
        private Color _player2Color;
        private int _player1Walls;
        private int _player2Walls;
        private bool _isPlacingWall;
        private Wall? _currentWall;

        // Logique métier QuoridorLib
        private GameManager _gameManager;
        private Board _board;
        private Player _player1;
        private Player _player2;

        public string Player1Name { get => _player1Name; set { _player1Name = value; OnPropertyChanged(); } }
        public string Player2Name { get => _player2Name; set { _player2Name = value; OnPropertyChanged(); } }
        public Color Player1Color { get => _player1Color; set { _player1Color = value; OnPropertyChanged(); } }
        public Color Player2Color { get => _player2Color; set { _player2Color = value; OnPropertyChanged(); } }
        public int Player1Walls { get => _player1Walls; set { _player1Walls = value; OnPropertyChanged(); } }
        public int Player2Walls { get => _player2Walls; set { _player2Walls = value; OnPropertyChanged(); } }
        public string CurrentPlayerName => _gameManager?.GetCurrentPlayer()?.Name ?? "";
        public Color CurrentPlayerColor
        {
            get
            {
                var current = _gameManager?.GetCurrentPlayer();
                if (current == null) return Colors.Black;
                return current == _player1 ? Player1Color : Player2Color;
            }
        }

        public PlayingPageViewModel(GameParameters parameters)
        {
            FlatMatrix = new ObservableCollection<CellContent>();
            for (int i = 0; i < 81; i++)
                FlatMatrix.Add(new CellContent { Symbol = "", Color = null, IsMovePossible = false });

            // Initialisation à partir des paramètres
            Player1Name = parameters.Player1Name;
            Player2Name = parameters.Player2Name;
            Player1Color = parameters.Player1Color;
            Player2Color = parameters.Player2Color;
            Player1Walls = parameters.NumberOfWalls;
            Player2Walls = parameters.NumberOfWalls;

            // Initialisation logique métier
            _player1 = new Player(Player1Name);
            _player2 = new Player(Player2Name);
            StubLoadManager stubLoad = new();
            _gameManager = new GameManager(stubLoad, new StubSaveManager(stubLoad));
            _gameManager.InitGame(_player1, _player2, parameters.BestOf);
            _board = _gameManager.GetCurrentRound().GetBoard();

            // Initialisation des commandes
            CellClickedCommand = new Command<CellContent>(OnCellClicked);
            PlaceWallCommand = new Command(OnPlaceWall);
            UndoCommand = new Command(OnUndo);

            SyncWithBoard();
            ShowPossibleMoves();
        }

        // Constructeur par défaut pour le design-time ou preview
        public PlayingPageViewModel() : this(new GameParameters())
        {
        }

        private void SyncWithBoard()
        {
            for (int i = 0; i < 81; i++)
                FlatMatrix[i] = new CellContent { Symbol = "", Color = null, IsMovePossible = false };

            // Placer les pions
            var pawns = _board.GetPawnsPositions();
            foreach (var kvp in pawns)
            {
                var pos = kvp.Value;
                var player = kvp.Key;
                string symbol = player == _player1 ? "1" : "2";
                Color color = player == _player1 ? Player1Color : Player2Color;
                SetCell(pos.GetPositionX(), pos.GetPositionY(), symbol, color);
            }

            // Placer les murs
            var walls = _board.GetWalls();
            foreach (var wall in walls)
            {
                var pos = wall.GetPosition();
                string symbol = wall.IsHorizontal() ? "─" : "│";
                SetCell(pos.GetPositionX(), pos.GetPositionY(), symbol, Colors.Brown);
            }
        }

        private void ShowPossibleMoves()
        {
            var currentPlayer = _gameManager.GetCurrentPlayer();
            var pawn = currentPlayer == _player1 ? _board.Pawn1 : _board.Pawn2;
            var possibleMoves = _board.GetPossibleMoves(pawn)
                .Select(pos => (pos.GetPositionX(), pos.GetPositionY())).ToList();
            foreach (var cell in FlatMatrix)
                cell.IsMovePossible = false;
            foreach (var (x, y) in possibleMoves)
            {
                int index = (8 - y) * 9 + x;
                if (index >= 0 && index < FlatMatrix.Count)
                    FlatMatrix[index].IsMovePossible = true;
            }
        }

        public void SetCell(int x, int y, string value, Color? color = null, bool isMovePossible = false)
        {
            int index = (8 - y) * 9 + x;
            if (index >= 0 && index < FlatMatrix.Count)
            {
                FlatMatrix[index] = new CellContent { Symbol = value, Color = color, IsMovePossible = isMovePossible };
            }
        }

        private void OnCellClicked(CellContent cell)
        {
            if (!cell.IsMovePossible) return;

            int index = FlatMatrix.IndexOf(cell);
            int x = index % 9;
            int y = 8 - (index / 9);

            var currentPlayer = _gameManager.GetCurrentPlayer();
            bool moved = _board.MovePawn(currentPlayer == _player1 ? _board.Pawn1 : _board.Pawn2, new Position(x, y));
            if (moved)
            {
                _gameManager.PlayTurn();
                SyncWithBoard();
                ShowPossibleMoves();
                OnPropertyChanged(nameof(CurrentPlayerName));
                OnPropertyChanged(nameof(CurrentPlayerColor));

                // Vérifier la victoire
                if (_board.IsWinner(currentPlayer == _player1 ? _board.Pawn1 : _board.Pawn2))
                {
                    // TODO: Afficher la page de fin de partie
                }
            }
        }

        private void OnPlaceWall()
        {
            _isPlacingWall = !_isPlacingWall;
            if (_isPlacingWall)
            {
                // TODO: Afficher les positions possibles pour placer un mur
            }
            else
            {
                _currentWall = null;
            }
        }

        private void OnUndo()
        {
            // TODO: Implémenter l'annulation du dernier coup
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 