using QuoridorLib.Managers;

namespace QuoridorMaui.Pages;

public partial class LeaderBoardPage : ContentPage
{
    public StubLoadManager loadManager=new();

    public LeaderBoardPage()
    {
        InitializeComponent();
        loadManager.LoadPlayers();
        BindingContext = loadManager;
    }
}
