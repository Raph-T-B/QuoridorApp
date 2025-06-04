using System.Globalization;
using QuoridorLib.Models;

namespace QuoridorMaui.Views;

public partial class SauvegardeListItem : ContentView
{


    

    public static readonly BindableProperty TheGameProperty =
        BindableProperty.Create(nameof(TheGame),
                                typeof(Game),
                                typeof(SauvegardeListItem));

    public Game TheGame
    {
        get => (Game)GetValue(SauvegardeListItem.TheGameProperty);
        set => SetValue(SauvegardeListItem.TheGameProperty, value);
    }

    public int boGlobal;
    public int boP1;
    public int boP2;
    public string Player1;
    public string Player2;


    public SauvegardeListItem()
	{
		InitializeComponent();
        if (TheGame != null)
        {
            boGlobal = TheGame.GetBestOf().GetNumberOfGames();
            boP2 = TheGame.GetBestOf().GetPlayer2Score();
            boP1 = TheGame.GetBestOf().GetPlayer1Score();
            Player1 = TheGame.GetPlayers()[0].Name;
            Player2 = TheGame.GetPlayers()[1].Name;
            Player1 = "pioupiou";
        }
        else
        {
            Player1 = "pioupliiiiii";
        }

        BindingContext = this;
    }
}