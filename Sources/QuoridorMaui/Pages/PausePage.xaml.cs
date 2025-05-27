namespace QuoridorMaui.Pages;

public partial class PausePage : ContentPage
{
    private void test_bouton(object sender, EventArgs e)
    {
        DisplayAlert("Confirmer", "coucou", "ok");
    }

    public PausePage()
    {
        InitializeComponent();
    }

    private async void Reprendre_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void Sauvegarder_Tapped(object sender, EventArgs e)
    {
        // TODO: Implémenter la sauvegarde
        await DisplayAlert("Sauvegarde", "Partie sauvegardée avec succès", "OK");
    }

    private async void Menu_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("homepage");
    }

    private async void Quitter_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}