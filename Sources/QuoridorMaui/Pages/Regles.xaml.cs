namespace QuoridorMaui.Pages;

public partial class Regles : ContentPage
{
    public Regles()
    {
        InitializeComponent();
    }

    private async void Retour_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//homepage");
    }
}