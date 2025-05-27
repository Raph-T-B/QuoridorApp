using System;
using Microsoft.Maui.Controls;

namespace QuoridorMaui.Pages
{
    public partial class Page1VS1 : ContentPage
    {
        public Page1VS1()
        {
            InitializeComponent();
        }

        private void Bo1_Tapped(object sender, EventArgs e)
        {
            // Logique pour Bo1
        }
        private void Bo3_Tapped(object sender, EventArgs e)
        {
            // Logique pour Bo3
        }
        private void Bo5_Tapped(object sender, EventArgs e)
        {
            // Logique pour Bo5
        }
        private void LancerPartie_Tapped(object sender, EventArgs e)
        {
            // Logique pour lancer la partie
        }
        private async void Retour_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}