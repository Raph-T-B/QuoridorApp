using QuoridorMaui.Models;
using QuoridorLib.Models;
using QuoridorStub.Stub;
using QuoridorLib.Managers;
using System.Collections.ObjectModel;
using QuoridorLib.Interfaces;

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
	private string _currentWallOrientation;


	public PlayingPage(GameParameters parameters)
	{
		Parameters = parameters;
		GameBoard = new GameBoard(parameters.Player1Color, parameters.Player2Color);
		StubLoadManager stubloadmanger = new();
		_gameManager = new GameManager(stubloadmanger,new StubSaveManager(stubloadmanger));

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
		if (walls1Label != null) walls1Label.Text = Parameters.NumberOfWalls.ToString();
		if (walls2Label != null) walls2Label.Text = Parameters.NumberOfWalls.ToString();

		// Initialiser l'affichage des pions
		var currentRound = _game.GetCurrentRound();
		if (currentRound != null)
		{
			var board = currentRound.GetBoard();
			var players = _game.GetPlayers();
			
			// Placer le pion 1
			var pawn1Pos = board.Pawn1.GetPosition();
			GameBoard.SetCell(pawn1Pos.GetPositionX(), pawn1Pos.GetPositionY(), "1", Player1Color);
			
			// Placer le pion 2
			var pawn2Pos = board.Pawn2.GetPosition();
			GameBoard.SetCell(pawn2Pos.GetPositionX(), pawn2Pos.GetPositionY(), "2", Player2Color);
		

			// Afficher les mouvements possibles pour le joueur actuel
			UpdatePossibleMoves();
		}
	}

    public PlayingPage(Game game)
    {
		// mettre les couleurs à jour !!!!!!
		// nous n'avons pas non plus les 
        GameBoard = new GameBoard(Colors.Red, Colors.Green);
        StubLoadManager stubloadmanger = new();
        _gameManager = new GameManager(stubloadmanger, new StubSaveManager(stubloadmanger));
		_gameManager.LoadGame(game);


        InitializeComponent();
        BindingContext = this;

        // Mettre à jour les labels des joueurs
        var player1Label = this.FindByName<Label>("Player1Label");
        var player2Label = this.FindByName<Label>("Player2Label");
        if (player1Label != null) player1Label.Text = _gameManager.GetPlayers()[0].Name;
        if (player2Label != null) player2Label.Text = _gameManager.GetPlayers()[1].Name;

        // Mettre à jour les murs restants
		/*
        var walls1Label = this.FindByName<Label>("Walls1Label");
        var walls2Label = this.FindByName<Label>("Walls2Label");
        if (walls1Label != null) walls1Label.Text = Parameters.NumberOfWalls.ToString();
        if (walls2Label != null) walls2Label.Text = Parameters.NumberOfWalls.ToString();
		*/
        // Initialiser l'affichage des pions
        var currentRound = _gameManager.GetCurrentRound();
        if (currentRound != null)
        {
            var board = currentRound.GetBoard();
            var players = _game.GetPlayers();

            // Placer le pion 1
            var pawn1Pos = board.Pawn1.GetPosition();
            GameBoard.SetCell(pawn1Pos.GetPositionX(), pawn1Pos.GetPositionY(), "1", Player1Color);

            // Placer le pion 2
            var pawn2Pos = board.Pawn2.GetPosition();
            GameBoard.SetCell(pawn2Pos.GetPositionX(), pawn2Pos.GetPositionY(), "2", Player2Color);


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
			.Select(p => (p.GetPositionX(), p.GetPositionY()))
			.ToList();

		GameBoard.UpdatePossibleMoves(possibleMoves);
	}

	private async void Pause_Clicked(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new PausePage(_gameManager));
	}

    private void OnCellTapped(object sender, TappedEventArgs e)
    {
        if (sender is Border border && border.BindingContext is CellContent cell)
        {
            int index = GameBoard.FlatMatrix.IndexOf(cell);
            int row = index / GameBoard.NbColumns;
            int col = index % GameBoard.NbColumns;

            // Convertir les coordonnées de la matrice (qui est inversée verticalement)
            int x = col;
            int y = GameBoard.NbRows - 1 - row;

            // Récupérer le joueur courant et son pion
            var currentPlayer = _game.CurrentPlayer;
            if (currentPlayer == null) return;

            var currentRound = _game.GetCurrentRound();
            if (currentRound == null) return;

            var board = currentRound.GetBoard();
            var players = _game.GetPlayers();
            var pawn = currentPlayer == players[0] ? board.Pawn1 : board.Pawn2;

            if (_isPlacingWall)
            {
                HandleWallPlacement(currentRound, x, y);
            }
            else
            {
                HandlePawnMovement(currentRound, currentPlayer, players, x, y);
            }
        }
    }

    private void HandlePawnMovement(Round currentRound, Player currentPlayer, ReadOnlyCollection<Player> players, int x, int y)
	{
		var board = currentRound.GetBoard();
		var pawn = currentPlayer == players[0] ? board.Pawn1 : board.Pawn2;
		var possibleMoves = board.GetPossibleMoves(pawn);

		if (possibleMoves.Any(p => p.GetPositionX() == x && p.GetPositionY() == y))
		{
			// Sauvegarder l'ancienne position
			var oldPosition = pawn.GetPosition();
			
			// Déplacer le pion
			bool moved = currentRound.MovePawn(x, y);
			if (moved)
			{
				// Effacer l'ancienne position
				GameBoard.SetCell(oldPosition.GetPositionX(), oldPosition.GetPositionY(), "", null);
				
				// Mettre à jour la nouvelle position
				GameBoard.SetCell(x, y, currentPlayer == players[0] ? "1" : "2", 
					currentPlayer == players[0] ? Player1Color : Player2Color);
				
				// Vérifier si le joueur a gagné
				if ((currentPlayer == players[0] && x == 8) || 
					(currentPlayer == players[1] && x == 0))
				{
					HandleWinningMove(currentPlayer);
				}
				else
				{
					// Passer au joueur suivant
					_gameManager.PlayTurn();
					// Mettre à jour le joueur courant dans le Game
					var nextPlayer = _gameManager.GetCurrentPlayer();
					if (nextPlayer != null)
					{
						currentRound.SwitchCurrentPlayer(nextPlayer);
					}
					// Mettre à jour les mouvements possibles pour le nouveau joueur
					UpdatePossibleMoves();
				}
			}
		}
	}

	private void HandleWallPlacement(Round currentRound, int x, int y)
	{
		try
		{
			if (currentRound.PlacingWall(x, y, _currentWallOrientation))
			{
				// Mettre à jour l'affichage des murs
				UpdateWallsDisplay(currentRound.GetBoard());
				// Passer au joueur suivant
				_gameManager.PlayTurn();
				// Mettre à jour le joueur courant dans le Game
				var nextPlayer = _gameManager.GetCurrentPlayer();
				if (nextPlayer != null)
				{
					currentRound.SwitchCurrentPlayer(nextPlayer);
				}
				// Mettre à jour les mouvements possibles
				UpdatePossibleMoves();
				// Réinitialiser le mode placement de mur
				_isPlacingWall = false;
				_currentWallOrientation = null;
			}
			else
			{
				DisplayAlert("Erreur", "Placement de mur invalide. Vérifiez qu'il n'y a pas de mur qui se croise ou qui se chevauche.", "OK");
			}
		}
		catch (Exception ex)
		{
			DisplayAlert("Erreur", $"Erreur lors du placement du mur : {ex.Message}", "OK");
		}
	}

	private void HandleWinningMove(Player currentPlayer)
	{
		var bestOf = _game.GetBestOf();
		DisplayAlert("Victoire !", $"{currentPlayer.Name} a gagné la manche !\nScore actuel - Joueur 1: {bestOf.GetPlayer1Score()}, Joueur 2: {bestOf.GetPlayer2Score()}", "OK");
		
		if (_gameManager.IsGameFinished())
		{
			DisplayGameOver();
		}
		else
		{
			// Commencer une nouvelle manche
			var players = _game.GetPlayers();
			_gameManager.InitGame(players[0], players[1]);
			// Réinitialiser l'affichage
			InitializeGameDisplay();
		}
	}

	private void DisplayGameOver()
	{
		var bestOf = _game.GetBestOf();
		DisplayAlert("Partie terminée", 
			$"Score final:\nJoueur 1: {bestOf.GetPlayer1Score()}\nJoueur 2: {bestOf.GetPlayer2Score()}", 
			"OK");
	}

	private void InitializeGameDisplay()
	{
		var currentRound = _game.GetCurrentRound();
		if (currentRound != null)
		{
			var board = currentRound.GetBoard();
			var players = _game.GetPlayers();
			
			// Réinitialiser le plateau
			GameBoard.InitializeBoard(Player1Color, Player2Color);
			
			// Placer les pions
			var pawn1Pos = board.Pawn1.GetPosition();
			GameBoard.SetCell(pawn1Pos.GetPositionX(), pawn1Pos.GetPositionY(), "1", Player1Color);
			
			var pawn2Pos = board.Pawn2.GetPosition();
			GameBoard.SetCell(pawn2Pos.GetPositionX(), pawn2Pos.GetPositionY(), "2", Player2Color);
			
			// Mettre à jour les murs
			UpdateWallsDisplay(board);
			
			// Mettre à jour les mouvements possibles
			UpdatePossibleMoves();
		}
	}

	private void UpdateWallsDisplay(Board board)
	{
		var walls = board.GetWallsPositions();
		foreach (var (p1, p2) in walls)
		{
			// Déterminer si le mur est horizontal ou vertical
			bool isHorizontal = p1.GetPositionY() == p2.GetPositionY();
			string symbol = isHorizontal ? "─" : "│";
			
			// Placer le mur à la position p1
			GameBoard.SetCell(p1.GetPositionX(), p1.GetPositionY(), symbol, Colors.Brown);
			
			// Si le mur est vertical, placer aussi à p2
			if (!isHorizontal)
			{
				GameBoard.SetCell(p2.GetPositionX(), p2.GetPositionY(), symbol, Colors.Brown);
			}
		}
	}

	private void OnHorizontalWall_Clicked(object sender, EventArgs e)
	{
		_isPlacingWall = true;
		_currentWallOrientation = "horizontal";
		DisplayAlert("Placement de mur", "Cliquez sur une case pour placer un mur horizontal", "OK");
	}

	private void OnVerticalWall_Clicked(object sender, EventArgs e)
	{
		_isPlacingWall = true;
		_currentWallOrientation = "vertical";
		DisplayAlert("Placement de mur", "Cliquez sur une case pour placer un mur vertical", "OK");
	}
}