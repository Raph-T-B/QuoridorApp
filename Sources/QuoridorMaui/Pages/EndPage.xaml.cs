using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using QuoridorMaui.Views;

namespace QuoridorMaui.Pages;

public partial class EndPage : ContentPage
{
    public EndPage()
    {
        InitializeComponent();
    }

    private async void NouvellePartie_Tapped(object sender, TappedEventArgs e)
    {
        // TODO: Implémenter la navigation vers la page de configuration de nouvelle partie
        await Navigation.PopToRootAsync();
    }
    
    private async void OnDetailsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("homepage");
    }
    
    private async void Retour_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}