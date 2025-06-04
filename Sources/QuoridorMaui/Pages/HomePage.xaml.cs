using QuoridorMaui.Views;

namespace QuoridorMaui.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void Jouer_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChoisirpartiePage());
    }

    private async void Leaderboard_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LeaderBoardPage());
    }

    private async void Regles_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Regles());
    }

    private void Quitter_Clicked(object sender, EventArgs e)
    {
        Application.Current.Quit();
    }
}