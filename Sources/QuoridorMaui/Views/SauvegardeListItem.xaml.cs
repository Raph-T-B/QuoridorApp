using System.ComponentModel;
using System.Globalization;
using QuoridorLib.Models;

namespace QuoridorMaui.Views;

public partial class SauvegardeListItem : ContentView
{
    public static readonly BindableProperty TheGameProperty =
       BindableProperty.Create(nameof(TheGame),
                               typeof(Game),
                               typeof(SauvegardeListItem),
                               propertyChanged: OnTheGameChanged);

    public Game TheGame
    {
        get => (Game)GetValue(SauvegardeListItem.TheGameProperty);
        set => SetValue(SauvegardeListItem.TheGameProperty, value);

    }

    public static readonly BindableProperty BoGlobalProperty =
      BindableProperty.Create(nameof(BoGlobal), typeof(int), typeof(SauvegardeListItem));

    public int BoGlobal
    {
        get => (int)GetValue(BoGlobalProperty);
        set => SetValue(BoGlobalProperty, value);
    }

    public static readonly BindableProperty BoP1Property =
        BindableProperty.Create(nameof(BoP1),
                                typeof(int),
                                typeof(SauvegardeListItem));

    public int BoP1
    {
        get => (int)GetValue(BoP1Property);
        set => SetValue(BoP1Property, value);

    }

    public static readonly BindableProperty BoP2Property =
        BindableProperty.Create(nameof(BoP2), typeof(int), typeof(SauvegardeListItem));

    public int BoP2
    {
        get => (int)GetValue(BoP2Property);
        set => SetValue(BoP2Property, value);

    }

    public static readonly BindableProperty Player1Property =
        BindableProperty.Create(nameof(Player1), typeof(string), typeof(SauvegardeListItem));

    public string Player1
    {
        get => (string)GetValue(Player1Property);
        set => SetValue(Player1Property, value);
    }

    public static readonly BindableProperty Player2Property =
        BindableProperty.Create(nameof(Player2), typeof(string), typeof(SauvegardeListItem));

    public string Player2
    {
        get => (string)GetValue(Player2Property);
        set => SetValue(Player2Property, value);
        
    }





    private static void OnTheGameChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (SauvegardeListItem)bindable;
        control.UpdateGameData();
    }

    private void UpdateGameData()
    {
        Console.WriteLine($"[DEBUG] UpdateGameData called - TheGame = {TheGame}");
        if (TheGame != null)
        {
            BoGlobal = TheGame.GetBestOf().GetNumberOfGames();
            BoP1 = TheGame.GetBestOf().GetPlayer1Score();
            BoP2 = TheGame.GetBestOf().GetPlayer2Score();
            Player1 = TheGame.GetPlayers()[0].Name;
            Player2 = TheGame.GetPlayers()[1].Name;
        }
    }


    public SauvegardeListItem()
	{
        InitializeComponent();
    }
}