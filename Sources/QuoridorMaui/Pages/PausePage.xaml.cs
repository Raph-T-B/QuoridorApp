using System.Diagnostics;
using QuoridorLib.Interfaces;
using QuoridorLib.Models;
using Persistence.Persistence;
 

namespace QuoridorMaui.Pages;

public partial class PausePage : ContentPage
{
    private IGameManager _gameManager;
    private IPlayersPersistence _playersPersistence;
    private IGamesPersistence _gamesPersistence;

    private async void Reprendre_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void Sauvegarder_Tapped(object sender, EventArgs e)
    {
        string pathGames = Path.Combine(FileSystem.AppDataDirectory, "Games.json");
        string pathPlayers = Path.Combine(FileSystem.AppDataDirectory, "Players.json");
        List<Game> games = _gamesPersistence.LoadGames(pathGames);
        List<Player> players = _playersPersistence.LoadPlayers(pathPlayers);
        _gameManager.SaveGames(games);
        _gameManager.SavePlayers(players); 
        _gameManager.SaveGame();
        _gameManager.SaveGamePlayers();
        games = _gameManager.LoadedGames();
        players = _gameManager.LoadedPlayers();
        _playersPersistence.SavePlayers(players, pathPlayers);
        _gamesPersistence.SaveGames(games, pathGames);
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
        _gamesPersistence = new GamePersistence();
        _playersPersistence = new PlayersPersistence();
		InitializeComponent();
	}
}