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
    
    private async void OnDetailsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("homepage");
    }

    private async void OnDetailsClikedChoix(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("choisirpartiepage");
    }

    private async void Quitter_Tapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}