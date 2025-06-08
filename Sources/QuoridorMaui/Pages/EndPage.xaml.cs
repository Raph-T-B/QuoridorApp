using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using QuoridorMaui.Views;
using Microsoft.Maui.Graphics;
using QuoridorMaui.Models;

namespace QuoridorMaui.Pages;

public partial class EndPage : ContentPage
{
    public string Player1Name { get; set; }
    public string Player2Name { get; set; }
    public Color Player1Color { get; set; }
    public Color Player2Color { get; set; }
    public int Player1Score { get; set; }
    public int Player2Score { get; set; }
    public string WinnerName { get; set; }
    public Color WinnerColor { get; set; }

    public EndPage(string player1Name, string player2Name, Color player1Color, Color player2Color, int player1Score, int player2Score, string winnerName, Color winnerColor)
    {
        InitializeComponent();
        Player1Name = player1Name;
        Player2Name = player2Name;
        Player1Color = player1Color;
        Player2Color = player2Color;
        Player1Score = player1Score;
        Player2Score = player2Score;
        WinnerName = winnerName;
        WinnerColor = winnerColor;

        // Met à jour l'affichage principal
        VictoryMessage.Text = $"Victoire de {WinnerName} !";
        VictoryMessage.TextColor = WinnerColor;

        // Met à jour l'affichage visuel des joueurs
        Player1ColorBox.Color = Player1Color;
        Player1NameLabel.Text = Player1Name;
        Player1ScoreLabel.Text = Player1Score.ToString();
        Player2ColorBox.Color = Player2Color;
        Player2NameLabel.Text = Player2Name;
        Player2ScoreLabel.Text = Player2Score.ToString();
    }

    private async void Rejouer_Tapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChoisirpartiePage());
    }

    private async void Menu_Tapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HomePage());
    }

    private void Quitter_Tapped(object sender, EventArgs e)
    {
#if ANDROID
        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#elif WINDOWS
#elif MACCATALYST
        UIKit.UIApplication.SharedApplication.PerformSelector(new ObjCRuntime.Selector("terminateWithSuccess"), null, 0f);
#endif
    }
}