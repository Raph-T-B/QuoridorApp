namespace QuoridorMaui.Pages;

public partial class PausePage : ContentPage
{
    private async void Reprendre_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("choisirpartiepage");
    }
    private async void Sauvegarder_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("choisirpartiepage");
    }
    private async void Menu_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("homepage");
    }
    private async void Quitter_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    public PausePage()
	{
		InitializeComponent();
	}
}