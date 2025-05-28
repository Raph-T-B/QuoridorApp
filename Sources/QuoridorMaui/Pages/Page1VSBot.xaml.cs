namespace QuoridorMaui.Pages;

public partial class Page1VSBot : ContentPage
{
    public Page1VSBot()
    {
        InitializeComponent();
    }

    private async void LancerPartie_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("gamepage");
    }

    private async void Retour_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}