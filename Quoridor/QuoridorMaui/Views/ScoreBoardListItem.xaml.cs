namespace QuoridorMaui.Views;

public partial class ScoreBoardListItem : ContentView
{
    

    public static readonly BindableProperty PlayerPlaceProperty = BindableProperty.Create(nameof(PlayerPlace), typeof(string), typeof(ScoreBoardListItem), string.Empty);
    public string PlayerPlace
    {
        get => (string)GetValue(ScoreBoardListItem.PlayerPlaceProperty);
        set => SetValue(ScoreBoardListItem.PlayerPlaceProperty, value);
    }

    public static readonly BindableProperty PlayerVictoryProperty = BindableProperty.Create(nameof(PlayerVictory), typeof(string), typeof(ScoreBoardListItem), string.Empty);
    public string PlayerVictory
    {
        get => (string)GetValue(ScoreBoardListItem.PlayerVictoryProperty);
        set => SetValue(ScoreBoardListItem.PlayerVictoryProperty, value);
    }

    public static readonly BindableProperty PlayerNameProperty = BindableProperty.Create(nameof(PlayerName), typeof(string), typeof(ScoreBoardListItem), string.Empty);

    public string PlayerName
    {
        get => (string)GetValue(ScoreBoardListItem.PlayerNameProperty);
        set => SetValue(ScoreBoardListItem.PlayerNameProperty, value);
    }
    public ScoreBoardListItem()
	{
		InitializeComponent();
	}
	
}