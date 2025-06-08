using Microsoft.Maui.Controls;
using QuoridorMaui.Models;
using QuoridorLib.Models;
using QuoridorLib.Managers;
using QuoridorLib.Interfaces;
using System.Collections.ObjectModel;
using QuoridorMaui.Views;

namespace QuoridorMaui.Pages;

public partial class PlayingPage : ContentPage
{
	public GameBoard GameBoard { get; }
	public GameParameters Parameters { get; }
	public Color Player1Color => Parameters.Player1Color;
	public Color Player2Color => Parameters.Player2Color;
	private readonly GameManager _gameManager;
	private Game _game;
	private bool _isPlacingWall;
	private string _currentWallOrientation = "vertical";
	private readonly Random _random = new();
	private int _player1Walls;
	private int _player2Walls;

	public PlayingPage(GameParameters parameters)
	{
		Parameters = parameters;
		GameBoard = new GameBoard(parameters.Player1Color, parameters.Player2Color);
		_gameManager = new GameManager(new StubLoadManager(), new StubSaveManager());
		_player1Walls = parameters.NumberOfWalls;
		_player2Walls = parameters.NumberOfWalls;
		
		// Créer les joueurs
		var player1 = new Player(Parameters.Player1Name);
		var player2 = new Player(Parameters.Player2Name);

		// Initialiser le jeu
		_gameManager.InitGame(player1, player2, Parameters.BestOf);
		_game = new Game(Parameters.BestOf);
		_game.AddPlayer(player1);
		_game.AddPlayer(player2);
		_game.LaunchRound();

		InitializeComponent();
		BindingContext = this;

		// Mettre à jour les labels des joueurs
		var player1Label = this.FindByName<Label>("Player1Label");
		var player2Label = this.FindByName<Label>("Player2Label");
		if (player1Label != null) player1Label.Text = Parameters.Player1Name;
		if (player2Label != null) player2Label.Text = Parameters.Player2Name;

		// Mettre à jour les murs restants
		var walls1Label = this.FindByName<Label>("Walls1Label");
		var walls2Label = this.FindByName<Label>("Walls2Label");
		if (walls1Label != null) walls1Label.Text = _player1Walls.ToString();
		if (walls2Label != null) walls2Label.Text = _player2Walls.ToString();

		// Initialiser l'affichage des pions
		var currentRound = _game.GetCurrentRound();
		if (currentRound != null)
		{
			var board = currentRound.GetBoard();
			var players = _game.GetPlayers();
			
			// Placer le pion 1 (inverser Y pour l'interface)
			var pawn1Pos = board.Pawn1.GetPosition();
			GameBoard.SetCell(pawn1Pos.GetPositionX(), 8 - pawn1Pos.GetPositionY(), "1", Player1Color);
			
			// Placer le pion 2 (inverser Y pour l'interface)
			var pawn2Pos = board.Pawn2.GetPosition();
			GameBoard.SetCell(pawn2Pos.GetPositionX(), 8 - pawn2Pos.GetPositionY(), "2", Player2Color);

			// Afficher les mouvements possibles pour le joueur actuel
			UpdatePossibleMoves();
		}
	}

	private void UpdatePossibleMoves()
	{
		var currentPlayer = _game.CurrentPlayer;
		if (currentPlayer == null) return;

		var currentRound = _game.GetCurrentRound();
		if (currentRound == null) return;

		var board = currentRound.GetBoard();
		var players = _game.GetPlayers();
		var pawn = currentPlayer == players[0] ? board.Pawn1 : board.Pawn2;
		
		var possibleMoves = board.GetPossibleMoves(pawn)
			.Select(p => (p.GetPositionX(), 8 - p.GetPositionY()))
			.ToList();

		GameBoard.UpdatePossibleMoves(possibleMoves);
	}

	private async void Pause_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new PausePage());
	}

	private void Wall_Clicked(object sender, EventArgs e)
	{
		_isPlacingWall = !_isPlacingWall;
		WallButton.BackgroundColor = _isPlacingWall ? Colors.Red : Colors.Blue;

		// Changer la couleur du plateau
		var frame = this.FindByName<Frame>("GameBoardFrame");
		if (frame != null)
			frame.BackgroundColor = _isPlacingWall ? Colors.Gold : Colors.DarkRed;
	}

	private void Orientation_Clicked(object sender, EventArgs e)
	{
		_currentWallOrientation = _currentWallOrientation == "vertical" ? "horizontal" : "vertical";
		OrientationButton.Texte = _currentWallOrientation == "vertical" ? "Vertical" : "Horizontal";
	}

	private void UpdateCellBorders(int x, int y, string orientation)
	{
		// Debug: afficher les coordonnées de placement
		System.Diagnostics.Debug.WriteLine($"UpdateCellBorders: placement {orientation} en interface ({x}, {y})");
		
		if (orientation == "horizontal")
		{
			// Mur horizontal bloque le mouvement vertical
			// Selon GetWallPositions: (x,y) à (x+1,y) et (x,y+1) à (x+1,y+1)
			// Doit bloquer le passage entre les lignes y et y+1
			
			// Bordure bas des cases (x,y) et (x+1,y)
			SetWallBorder(x, y, "BottomBorder", true);
			SetWallBorder(x + 1, y, "BottomBorder", true);
			// Bordure haut des cases (x,y+1) et (x+1,y+1)
			SetWallBorder(x, y + 1, "TopBorder", true);
			SetWallBorder(x + 1, y + 1, "TopBorder", true);
		}
		else // vertical
		{
			// Mur vertical bloque le mouvement horizontal
			// Selon GetWallPositions: (x,y) à (x,y+1) et (x+1,y) à (x+1,y+1)
			// Doit bloquer le passage entre les colonnes x et x+1
			
			// Bordure droite des cases (x,y) et (x,y+1)
			SetWallBorder(x, y, "RightBorder", true);
			SetWallBorder(x, y + 1, "RightBorder", true);
			// Bordure gauche des cases (x+1,y) et (x+1,y+1)
			SetWallBorder(x + 1, y, "LeftBorder", true);
			SetWallBorder(x + 1, y + 1, "LeftBorder", true);
		}
	}

	private void SetWallBorder(int x, int y, string borderName, bool isWall)
	{
		try
		{
			var matrixLayout = GameBoardFrame.Content as MyLayouts.MatrixLayout;
			if (matrixLayout == null) return;

			var cell = matrixLayout.GetCellAt(x, y);
			if (cell is Grid grid)
			{
				// Trouver la bordure spécifique par son nom
				var border = grid.FindByName<Border>(borderName);
				if (border != null)
				{
					if (isWall)
					{
						// Appliquer le style du mur
						border.Stroke = Colors.Black;
						border.StrokeThickness = 6;
						border.ZIndex = 10;
						
						// Ajuster la taille
						if (borderName == "TopBorder" || borderName == "BottomBorder")
						{
							border.HeightRequest = 6;
							border.HorizontalOptions = LayoutOptions.Fill;
						}
						else
						{
							border.WidthRequest = 6;
							border.VerticalOptions = LayoutOptions.Fill;
						}
					}
					else
					{
						border.Stroke = Colors.DarkRed;
						border.StrokeThickness = 1;
						border.ZIndex = 0;
					}
				}
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Erreur SetWallBorder: {ex.Message}");
		}
	}

	private View GetCellAt(int x, int y)
	{
		var matrixLayout = GameBoardFrame.Content as MyLayouts.MatrixLayout;
		if (matrixLayout != null)
		{
			return matrixLayout.GetCellAt(x, y);
		}
		return null;
	}

	private void OnCellTapped(object sender, EventArgs e)
	{
		var border = sender as Border;
		if (border == null) return;

		var cell = border.Parent as Grid;
		if (cell == null) return;

		var matrixLayout = GameBoardFrame.Content as MyLayouts.MatrixLayout;
		if (matrixLayout == null) return;

		var position = matrixLayout.GetCellPosition(cell);
		if (position == null) return;

		// Convertir les coordonnées interface vers la logique métier (inverser Y)
		int gameY = 8 - position.Value.y;

		var currentRound = _game.GetCurrentRound();
		if (currentRound == null) return;

		var board = currentRound.GetBoard();
		if (board == null) return;

		var currentPlayer = _game.CurrentPlayer;
		if (currentPlayer == null) return;

		var players = _game.GetPlayers();
		var pawn = currentPlayer == players[0] ? board.Pawn1 : board.Pawn2;

		if (_isPlacingWall)
		{
			// Vérifier si le joueur a encore des murs
			var wallsLabel = currentPlayer == players[0] ? Walls1Label : Walls2Label;
			if (wallsLabel != null && int.Parse(wallsLabel.Text) <= 0)
			{
				DisplayAlert("Erreur", "Vous n'avez plus de murs disponibles", "OK");
				return;
			}

			// Vérifier les limites du plateau avant de créer les murs
			if (position.Value.x + 1 >= 9 || gameY + 1 >= 9)
			{
				DisplayAlert("Erreur", "Position de mur invalide (trop près du bord)", "OK");
				return;
			}

			// Debug : afficher les coordonnées
			System.Diagnostics.Debug.WriteLine($"Tentative placement mur {_currentWallOrientation} en position interface ({position.Value.x}, {position.Value.y}) -> position logique ({position.Value.x}, {gameY})");
			
			// Utiliser la méthode standard PlacingWall de Round au lieu de créer manuellement
			if (currentRound.PlacingWall(position.Value.x, gameY, _currentWallOrientation))
			{
				// Debug : vérifier que le mur a été ajouté
				var wallsCount = board.GetWalls().Count;
				System.Diagnostics.Debug.WriteLine($"Mur placé avec succès ! Nombre total de murs: {wallsCount}");
				
				// Afficher le mur sur l'interface (coordonnées interface)
				System.Diagnostics.Debug.WriteLine($"Affichage mur {_currentWallOrientation} logique: ({position.Value.x}, {gameY}) -> interface: ({position.Value.x}, {position.Value.y})");
				UpdateCellBorders(position.Value.x, position.Value.y, _currentWallOrientation);
				
				// Mettre à jour le nombre de murs restants
				if (wallsLabel != null)
				{
					wallsLabel.Text = (int.Parse(wallsLabel.Text) - 1).ToString();
				}

				// Désactiver le mode placement de mur
				_isPlacingWall = false;
				var frame = this.FindByName<Frame>("GameBoardFrame");
				if (frame != null)
				{
					frame.BackgroundColor = Colors.DarkRed;
				}

				// Passer au joueur suivant
				_game.NextPlayer();
				UpdatePossibleMoves();
			}
			else
			{
				DisplayAlert("Erreur", "Impossible de placer le mur à cet endroit", "OK");
			}
		}
		else
		{
			HandlePawnMovement(currentRound, currentPlayer, players, position.Value.x, gameY, position.Value.x, position.Value.y);
		}
	}

	private void HandlePawnMovement(Round currentRound, Player currentPlayer, ReadOnlyCollection<Player> players, int x, int y, int interfaceX, int interfaceY)
	{
		var board = currentRound.GetBoard();
		var pawn = currentPlayer == players[0] ? board.Pawn1 : board.Pawn2;
		var possibleMoves = board.GetPossibleMoves(pawn);

		// Conversion Y interface -> logique
		int gameY = 8 - y;

		// Debug : vérifier le déplacement tenté
		System.Diagnostics.Debug.WriteLine($"Tentative déplacement de {currentPlayer.Name} vers logique ({x}, {gameY}) depuis interface ({8-y}, {x})");
		System.Diagnostics.Debug.WriteLine($"Position actuelle logique: ({pawn.GetPositionX()}, {pawn.GetPositionY()}) -> interface: ({pawn.GetPositionX()}, {8 - pawn.GetPositionY()})");
		System.Diagnostics.Debug.WriteLine($"Mouvements possibles logiques: {string.Join(", ", possibleMoves.Select(p => $"({p.GetPositionX()}, {p.GetPositionY()})"))}");

		if (possibleMoves.Any(p => p.GetPositionX() == x && p.GetPositionY() == gameY))
		{
			System.Diagnostics.Debug.WriteLine("Mouvement autorisé !");

			// Sauvegarder l'ancienne position
			var oldPosition = pawn.GetPosition();

			// Déplacer le pion
			var newPosition = new Position(x, gameY);
			if (board.MovePawn(pawn, newPosition))
			{
				// Effacer l'ancienne position (inverser Y pour l'interface)
				GameBoard.ClearCell(oldPosition.GetPositionX(), 8 - oldPosition.GetPositionY());

				// Mettre à jour la nouvelle position (inverser Y pour l'interface)
				GameBoard.SetCell(x, 8 - gameY, currentPlayer == players[0] ? "1" : "2",
					currentPlayer == players[0] ? Player1Color : Player2Color);

				// Vérifier si le joueur a gagné
				if (board.IsWinner(pawn))
				{
					HandleWinningMove(currentPlayer);
				}
				else
				{
					_game.NextPlayer();
					UpdatePossibleMoves();
				}
			}
		}
		else
		{
			System.Diagnostics.Debug.WriteLine("Mouvement BLOQUÉ !");
		}
	}

	private void HandleWallPlacement(Round currentRound, int x, int y)
	{
		var board = currentRound.GetBoard();
		if (board == null) return;

		var currentPlayer = _game.CurrentPlayer;
		var players = _game.GetPlayers();
		bool isPlayer1 = currentPlayer == players[0];

		// Vérifier si le joueur a encore des murs
		if ((isPlayer1 && _player1Walls <= 0) || (!isPlayer1 && _player2Walls <= 0))
		{
			return;
		}

		// Créer les murs en fonction de l'orientation
		Wall wall1, wall2;
		if (_currentWallOrientation == "vertical")
		{
			wall1 = new Wall(x, y, x, y + 1);
			wall2 = new Wall(x, y + 1, x, y + 2);
		}
		else // horizontal
		{
			wall1 = new Wall(x, y, x + 1, y);
			wall2 = new Wall(x + 1, y, x + 2, y);
		}

		// Essayer de placer les murs
		if (board.AddCoupleWall(wall1, wall2, _currentWallOrientation))
		{
			// Mettre à jour le nombre de murs restants
			if (isPlayer1)
			{
				_player1Walls--;
				var walls1Label = this.FindByName<Label>("Walls1Label");
				if (walls1Label != null) walls1Label.Text = _player1Walls.ToString();
			}
			else
			{
				_player2Walls--;
				var walls2Label = this.FindByName<Label>("Walls2Label");
				if (walls2Label != null) walls2Label.Text = _player2Walls.ToString();
			}

			// Désactiver le mode placement de mur
			_isPlacingWall = false;
			WallButton.BackgroundColor = Colors.Blue;

			// Passer au tour suivant
			_game.NextPlayer();
			UpdatePossibleMoves();
		}

		// Après la pose d'un mur et la désactivation du mode placement
		// (dans OnCellTapped ou HandleWallPlacement, là où _isPlacingWall = false)
		var frame = this.FindByName<Frame>("GameBoardFrame");
		if (frame != null)
			frame.BackgroundColor = Colors.Transparent;
	}

	private void HandleWinningMove(Player winner)
	{
		// Le score est mis à jour automatiquement dans MovePawn
		// On vérifie si la partie est terminée
		if (_game.IsGameOver())
		{
			Navigation.PushAsync(new EndPage());
		}
		else
		{
			_game.LaunchRound();
			ResetBoard();
		}
	}

	private void ResetBoard()
	{
		// Réinitialiser les murs
		_player1Walls = Parameters.NumberOfWalls;
		_player2Walls = Parameters.NumberOfWalls;
		var walls1Label = this.FindByName<Label>("Walls1Label");
		var walls2Label = this.FindByName<Label>("Walls2Label");
		if (walls1Label != null) walls1Label.Text = _player1Walls.ToString();
		if (walls2Label != null) walls2Label.Text = _player2Walls.ToString();

		// Réinitialiser l'affichage
		GameBoard.InitializeBoard(Player1Color, Player2Color);

		var currentRound = _game.GetCurrentRound();
		if (currentRound != null)
		{
			var board = currentRound.GetBoard();
			
			// Placer les pions
			var pawn1Pos = board.Pawn1.GetPosition();
			GameBoard.SetCell(pawn1Pos.GetPositionX(), 8 - pawn1Pos.GetPositionY(), "1", Player1Color);
			
			var pawn2Pos = board.Pawn2.GetPosition();
			GameBoard.SetCell(pawn2Pos.GetPositionX(), 8 - pawn2Pos.GetPositionY(), "2", Player2Color);

			UpdatePossibleMoves();
		}
	}

	private void HandleCellClick(int x, int y)
	{
		var currentRound = _game.GetCurrentRound();
		if (currentRound == null) return;

		var board = currentRound.GetBoard();
		if (board == null) return;

		var currentPlayer = _game.CurrentPlayer;
		if (currentPlayer == null) return;

		var players = _game.GetPlayers();
		var pawn = currentPlayer == players[0] ? board.Pawn1 : board.Pawn2;

		if (_isPlacingWall)
		{
			HandleWallPlacement(currentRound, x, y);
		}
		else
		{
			var possibleMoves = board.GetPossibleMoves(pawn)
				.Select(p => (p.GetPositionX(), 8 - p.GetPositionY()))
				.ToList();

			if (possibleMoves.Contains((x, y)))
			{
				// Sauvegarder l'ancienne position
				var oldPosition = pawn.GetPosition();
				
				// Déplacer le pion
				var newPosition = new Position(x, y);
				if (board.MovePawn(pawn, newPosition))
				{
					// Effacer l'ancienne position (inverser Y pour l'interface)
					GameBoard.ClearCell(oldPosition.GetPositionX(), 8 - oldPosition.GetPositionY());
					
					// Mettre à jour la nouvelle position (inverser Y pour l'interface)
					GameBoard.SetCell(x, 8-y, currentPlayer == players[0] ? "1" : "2", 
						currentPlayer == players[0] ? Player1Color : Player2Color);
					
					// Vérifier si le joueur a gagné
					if (board.IsWinner(pawn))
					{
						HandleWinningMove(currentPlayer);
					}
					else
					{
						// Passer au tour suivant
						_game.NextPlayer();
						UpdatePossibleMoves();
					}
				}
			}
		}
	}
}