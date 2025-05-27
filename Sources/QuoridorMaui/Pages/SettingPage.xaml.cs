namespace QuoridorMaui.Pages;

public partial class SettingPage : ContentPage
{
    private void test_bouton(object sender, EventArgs e)
    {
        DisplayAlert("Confirmer", "coucou", "ok");

    }
    

    private void OnThemeButtonClicked(object sender, EventArgs e)
    {
        if (sender == BtnSombre)
        {
            BtnSombre.BackgroundColor = Colors.LightSalmon;
            BtnSombre.TextColor = Colors.White;

            BtnClair.BackgroundColor = Colors.White;
            BtnClair.TextColor = Colors.Black;
        }
        else if (sender == BtnClair)
        {
            BtnClair.BackgroundColor = Colors.LightSalmon;
            BtnClair.TextColor = Colors.White;

            BtnSombre.BackgroundColor = Colors.White;
            BtnSombre.TextColor = Colors.Black;
        }
    }
    private void OnDeplacementButtonClicked(object sender, EventArgs e)
    {
        if (sender == BtnShowDep)
        {
            BtnShowDep.BackgroundColor = Colors.LightSalmon;
            BtnShowDep.TextColor = Colors.White;

            BtnHideDep.BackgroundColor = Colors.White;
            BtnHideDep.TextColor = Colors.Black;
        }
        else if (sender == BtnHideDep)
        {
            BtnHideDep.BackgroundColor = Colors.LightSalmon;
            BtnHideDep.TextColor = Colors.White;

            BtnShowDep.BackgroundColor = Colors.White;
            BtnShowDep.TextColor = Colors.Black;
        }
    }

    private void OnMurButtonClicked(object sender, EventArgs e)
    {
        if (sender == BtnShowWall)
        {
            BtnShowWall.BackgroundColor = Colors.LightSalmon;
            BtnShowWall.TextColor = Colors.White;

            BtnHideWall.BackgroundColor = Colors.White;
            BtnHideWall.TextColor = Colors.Black;
        }
        else if (sender == BtnHideWall)
        {
            BtnHideWall.BackgroundColor = Colors.LightSalmon;
            BtnHideWall.TextColor = Colors.White;

            BtnShowWall.BackgroundColor = Colors.White;
            BtnShowWall.TextColor = Colors.Black;
        }
    }

    private async void Retour_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    public SettingPage()
	{
		InitializeComponent();
	}
}