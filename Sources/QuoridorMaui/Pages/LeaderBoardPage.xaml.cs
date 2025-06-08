using Persistence.Persistence;
using QuoridorLib.Managers;
using QuoridorMaui.Models;
using QuoridorStub.Stub;
namespace QuoridorMaui.Pages;

public partial class LeaderBoardPage : ContentPage
{
    public PlayersPersistence playersPersistence = new();
    public StubLoadManager loadManager = new();

    private async void Retour_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    public LeaderBoardPage()
    {
        InitializeComponent();
        string pathPlayers = Path.Combine(FileSystem.AppDataDirectory, "Players.json");
        loadManager.LoadPlayers(playersPersistence.LoadPlayers(pathPlayers));
        BindingContext = loadManager;

    }

}
