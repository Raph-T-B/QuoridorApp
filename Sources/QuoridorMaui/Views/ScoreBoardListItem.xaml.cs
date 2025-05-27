using System.Reflection.Metadata;
using QuoridorLib.Models;

namespace QuoridorMaui.Views;

public partial class ScoreBoardListItem : ContentView
{

    public static readonly BindableProperty playerNameProperty = BindableProperty.Create(nameof(PlayerName), typeof(string), typeof(ScoreBoardListItem), string.Empty);

    public string PlayerName
    {
        get => (string)GetValue(ScoreBoardListItem.playerNameProperty);
        set => SetValue(ScoreBoardListItem.playerNameProperty, value);
    }

    public static readonly BindableProperty playerVictoriesProperty = BindableProperty.Create(nameof(PlayerVictories), typeof(int), typeof(ScoreBoardListItem), string.Empty);

    public int PlayerVictories
    {
        get => (int)GetValue(ScoreBoardListItem.playerVictoriesProperty);
        set => SetValue(ScoreBoardListItem.playerVictoriesProperty, value);
    }



    public ScoreBoardListItem()
	{
        InitializeComponent();
        BindingContext = this;
    }
	
}