using System.Reflection.Metadata;
using QuoridorLib.Models;

namespace QuoridorMaui.Views;

public partial class ScoreBoardListItem : ContentView
{

    public static readonly BindableProperty PlayerNameProperty = 
        BindableProperty.Create(nameof(PlayerName), 
                                typeof(string), 
                                typeof(ScoreBoardListItem), 
                                string.Empty);

    public string PlayerName
    {
        get => (string)GetValue(ScoreBoardListItem.PlayerNameProperty);
        set => SetValue(ScoreBoardListItem.PlayerNameProperty, value);
    }

    public static readonly BindableProperty PlayerVictoriesProperty = 
        BindableProperty.Create(nameof(PlayerVictories), 
                                typeof(uint), 
                                typeof(ScoreBoardListItem));

    public uint PlayerVictories
    {
        get => (uint)GetValue(ScoreBoardListItem.PlayerVictoriesProperty);
        set => SetValue(ScoreBoardListItem.PlayerVictoriesProperty, value);
    }



    public ScoreBoardListItem()
	{
        InitializeComponent();

        BindingContext = this;
    }
	
}