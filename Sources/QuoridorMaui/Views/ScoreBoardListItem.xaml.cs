using System.Reflection.Metadata;
using QuoridorLib.Models;

namespace QuoridorMaui.Views;

public partial class ScoreBoardListItem : ContentView
{

    public static readonly BindableProperty playerProperty = BindableProperty.Create(nameof(Player), typeof(Player), typeof(ScoreBoardListItem), string.Empty);

    public Player Player
    {
        get => (Player)GetValue(ScoreBoardListItem.playerProperty);
        set => SetValue(ScoreBoardListItem.playerProperty, value);
    }



    public ScoreBoardListItem()
	{
        InitializeComponent();
        BindingContext = this;
    }
	
}