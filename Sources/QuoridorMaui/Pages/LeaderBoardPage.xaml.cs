namespace QuoridorMaui.Pages;

public partial class LeaderBoardPage : ContentPage
{
	public LeaderBoardPage()
	{
        InitializeComponent();
	}

    private async void Retour_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("homepage");
    }
}