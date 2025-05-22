namespace QuoridorMaui.Pages;

public partial class ChoisirSauvegardePage : ContentPage
{


    private async void Retour_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("homepage");
    }
    bool isSelected = false;

    private void OnButtonClicked(object sender, EventArgs e)
    {
        isSelected = !isSelected;
        VisualStateManager.GoToState((Button)sender, isSelected ? "Selected" : "Normal");
    }

    private async void Retour_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    public ChoisirSauvegardePage()
    {
        InitializeComponent();
    }
}