using QuoridorMaui.Models;

namespace QuoridorMaui.Pages;

public partial class PlayingPage : ContentPage
{
	public GameBoard GameBoard { get; } = new GameBoard();
	public GameParameters Parameters { get; }

	public PlayingPage(GameParameters parameters)
	{
		Parameters = parameters;
		InitializeComponent();
		BindingContext = this;
		InitializeGame();
	}

	private void InitializeGame()
	{
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

		// Initialiser le plateau de jeu
		// TODO: Ajouter la logique d'initialisation du plateau
	}
}