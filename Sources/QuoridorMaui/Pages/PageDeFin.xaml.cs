using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace QuoridorMaui.Pages;

public partial class PageDeFin : ContentPage
{
    public PageDeFin()
    {
        InitializeComponent();
    }

    private async void NouvellePartie_Tapped(object sender, TappedEventArgs e)
    {
        // TODO: Implémenter la navigation vers la page de configuration de nouvelle partie
        await Navigation.PopToRootAsync();
    }

    private async void Retour_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}