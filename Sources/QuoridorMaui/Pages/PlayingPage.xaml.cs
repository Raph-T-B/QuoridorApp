using QuoridorMaui.Models;

namespace QuoridorMaui.Pages;

public partial class PlayingPage : ContentPage
{
	public GameBoard GameBoard { get; }
	public GameParameters Parameters { get; }
	public Color Player1Color => Parameters.Player1Color;
	public Color Player2Color => Parameters.Player2Color;

	public PlayingPage(GameParameters parameters)
	{
		Parameters = parameters;
		GameBoard = new GameBoard(parameters.Player1Color, parameters.Player2Color);
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

		// Initialiser le plateau de jeu (déjà fait via GameBoard)
	}

	private async void Pause_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new PausePage());
	}
}