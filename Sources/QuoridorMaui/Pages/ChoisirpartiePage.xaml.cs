namespace QuoridorMaui.Pages;

public partial class ChoisirpartiePage : ContentPage
{
    private void test_bouton(object sender, EventArgs e)
    {
        DisplayAlert("Confirmer", "coucou", "ok");

    }
    public ChoisirpartiePage()
	{
		InitializeComponent();
	}
    private async void choisirsauvegardepage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("choisirsauvegardepage");
    }

    private async void OnDetailsClikedChoix(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("choisirpartiepage");
    }
}