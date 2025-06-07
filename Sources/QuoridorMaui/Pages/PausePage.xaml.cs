
using QuoridorLib.Interfaces;
using QuoridorLib.Models;


namespace QuoridorMaui.Pages;

public partial class PausePage : ContentPage
{
    private IGameManager _gameManager;
    IPlayersPersistence _playersPersistence;
    IGamesPersistence _gamesPersistence;

    private async void Reprendre_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void Sauvegarder_Tapped(object sender, EventArgs e)
    {
        List<Game> games = _gamesPersistence.LoadGames("Games.txt");
        List<Player> players = _playersPersistence.LoadPlayers("Players.txt");
        _gameManager.SaveGames(games);
        _gameManager.SavePlayers(players); 
        _gameManager.SaveGame();
        _gameManager.SaveGamePlayers();
        games = _gameManager.LoadedGames();
        players = _gameManager.LoadedPlayers();
        _playersPersistence.SavePlayers(players, "Players.txt");
        _gamesPersistence.SaveGames(games, "Games.txt");
        await Navigation.PopToRootAsync();
    }
    private async void Regles_Tapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Regles());
    }
    private async void Menu_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("homepage");
    }
    private async void Quitter_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }

    public PausePage(IGameManager gameManager)
	{
        _gameManager = gameManager;
		InitializeComponent();
	}
}