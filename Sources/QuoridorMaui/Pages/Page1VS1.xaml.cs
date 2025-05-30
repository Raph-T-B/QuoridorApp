using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using QuoridorMaui.Models;

namespace QuoridorMaui.Pages
{
    public partial class Page1VS1 : ContentPage
    {
        private readonly List<string> couleurs = new() { "Bleu", "Rouge", "Vert", "Violet", "Rose", "Orange" };
        private readonly Dictionary<string, Color> couleurMap = new()
        {
            { "Bleu", Colors.Blue },
            { "Rouge", Colors.Red },
            { "Vert", Colors.Green },
            { "Violet", Colors.Purple },
            { "Rose", Color.FromArgb("#FF69B4") },
            { "Orange", Colors.Orange }
        };
        private string couleurJ1 = "Bleu";
        private string couleurJ2 = "Rouge";

        public GameBoard GameBoard { get; } = new GameBoard();

        public Page1VS1()
        {
            InitializeComponent();
            BindingContext = this;
            InitColorButtons();
        }

        private void InitColorButtons()
        {
            ColorButtonsJ1.Children.Clear();
            var btnJ1 = new Button
            {
                BackgroundColor = couleurMap[couleurJ1],
                CornerRadius = 25,
                WidthRequest = 50,
                HeightRequest = 50,
                BorderColor = Colors.Black,
                BorderWidth = 2,
                Text = "",
                TextColor = GetTextColor(couleurJ1)
            };
            btnJ1.Clicked += async (s, e) =>
            {
                string result = await DisplayActionSheet("Choisir la couleur du Joueur 1", null, null, couleurs.ToArray());
                if (!string.IsNullOrEmpty(result) && couleurMap.ContainsKey(result))
                {
                    couleurJ1 = result;
                    btnJ1.BackgroundColor = couleurMap[result];
                    btnJ1.TextColor = GetTextColor(result);
                }
            };
            ColorButtonsJ1.Children.Add(btnJ1);

            ColorButtonsJ2.Children.Clear();
            var btnJ2 = new Button
            {
                BackgroundColor = couleurMap[couleurJ2],
                CornerRadius = 25,
                WidthRequest = 50,
                HeightRequest = 50,
                BorderColor = Colors.Black,
                BorderWidth = 2,
                Text = "",
                TextColor = GetTextColor(couleurJ2)
            };
            btnJ2.Clicked += async (s, e) =>
            {
                string result = await DisplayActionSheet("Choisir la couleur du Joueur 2", null, null, couleurs.ToArray());
                if (!string.IsNullOrEmpty(result) && couleurMap.ContainsKey(result))
                {
                    couleurJ2 = result;
                    btnJ2.BackgroundColor = couleurMap[result];
                    btnJ2.TextColor = GetTextColor(result);
                }
            };
            ColorButtonsJ2.Children.Add(btnJ2);
        }

        private Color GetTextColor(string couleur)
        {
            return (couleur == "Orange" || couleur == "Rose") ? Colors.Black : Colors.White;
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