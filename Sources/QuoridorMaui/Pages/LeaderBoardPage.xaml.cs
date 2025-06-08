using QuoridorLib.Managers;

namespace QuoridorMaui.Pages;

public partial class LeaderBoardPage : ContentPage
{
    public StubLoadManager loadManager=new();

    private async void Retour_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    public LeaderBoardPage()
    {
        InitializeComponent();
        loadManager.LoadPlayers();
        BindingContext = loadManager;
    }

    private async void Retour_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("homepage");
    }
}
