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
        private int bestOf = 1;

        public Page1VS1()
        {
            InitializeComponent();
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
            bestOf = 1;
        }

        private void Bo3_Tapped(object sender, EventArgs e)
        {
            bestOf = 3;
        }

        private void Bo5_Tapped(object sender, EventArgs e)
        {
            bestOf = 5;
        }

        private async void LancerPartie_Tapped(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryJoueur1.Text) || string.IsNullOrWhiteSpace(EntryJoueur2.Text))
            {
                await DisplayAlert("Erreur", "Veuillez entrer les noms des deux joueurs", "OK");
                return;
            }

            if (!int.TryParse(EntryNbMurs.Text, out int nbMurs) || nbMurs <= 0)
            {
                await DisplayAlert("Erreur", "Veuillez entrer un nombre de murs valide", "OK");
                return;
            }

            var parameters = new GameParameters
            {
                Player1Name = EntryJoueur1.Text,
                Player2Name = EntryJoueur2.Text,
                Player1Color = couleurMap[couleurJ1],
                Player2Color = couleurMap[couleurJ2],
                NumberOfWalls = nbMurs,
                IsBotGame = false,
                BestOf = bestOf
            };

            await Navigation.PushAsync(new PlayingPage(parameters));
        }

        private async void Retour_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChoisirpartiePage());
        }
    }
}