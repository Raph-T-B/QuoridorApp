namespace QuoridorMaui.Pages;

public partial class ChoisirSauvegardePage : ContentPage
{


    private void test_bouton(object sender, EventArgs e)
    {
        DisplayAlert("Confirmer", "coucou", "ok");

    }
    bool isSelected = false;

    private void OnButtonClicked(object sender, EventArgs e)
    {
        isSelected = !isSelected;
        VisualStateManager.GoToState((Button)sender, isSelected ? "Selected" : "Normal");
    }

    public ChoisirSauvegardePage()
    {
        InitializeComponent();
    }
}