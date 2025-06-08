using QuoridorStub.Stub;
using Persistence.Persistence;
using System.Diagnostics;
using QuoridorMaui.Models;


namespace QuoridorMaui.Pages;

public partial class ChoisirSauvegardePage : ContentPage
{
    public GamePersistence gamePersistence =new();
    public StubLoadManager loadManager = new();
    public ListGames games=new();

    public ChoisirSauvegardePage()
    {
        InitializeComponent();
        string pathGames = Path.Combine(FileSystem.AppDataDirectory,"Games.json");
        loadManager.LoadGames(gamePersistence.LoadGames(pathGames));
        games.Load(loadManager.LoadedGames());
        
    }
    private async void Retour_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}