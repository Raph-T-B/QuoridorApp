using System;
using Microsoft.Maui.Controls;

namespace QuoridorMaui.Pages;
public partial class ChoisirpartiePage : ContentPage
{
    public ChoisirpartiePage()
	{
		InitializeComponent();
	}
    private async void ReprendrePartie_Tapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChoisirSauvegardePage());
    }

    private async void Page1VS1_Tapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Page1VS1());
    }



    private async void Retour_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();

    }

}
