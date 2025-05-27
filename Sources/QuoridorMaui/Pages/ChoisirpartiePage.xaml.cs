using System;
using Microsoft.Maui.Controls;

namespace QuoridorMaui.Pages;
public partial class ChoisirpartiePage : ContentPage
{
    public ChoisirpartiePage()
	{
		InitializeComponent();
	}
    private async void ChoisirSauvegardePage_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("choisirsauvegardepage");
    }

    private async void Page1VS1_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("page1vs1");
    }

    private async void UnVSBot_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("page1vsbot");
    }

    private async void Retour_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("homepage");
    }

    private async void ReprendrePartie_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("choisirsauvegardepage");
    }
}
