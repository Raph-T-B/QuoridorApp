namespace QuoridorMaui.Pages;

public partial class PausePage : ContentPage
{
    private async void Reprendre_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void Sauvegarder_Tapped(object sender, EventArgs e)
    {
        // TODO: Implémenter la sauvegarde
        await DisplayAlert("Information", "Partie sauvegardée", "OK");
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

    public PausePage()
	{
		InitializeComponent();
	}
}