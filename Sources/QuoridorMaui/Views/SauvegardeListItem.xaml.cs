using QuoridorLib.Models;
using QuoridorMaui.Models;

namespace QuoridorMaui.Views;
public partial class SauvegardeListItem : ContentView
{
    public static readonly BindableProperty TheGameProperty =
        BindableProperty.Create(nameof(TheGame), typeof(Game), typeof(SauvegardeListItem), propertyChanged: OnGameChanged);

    public Game TheGame
    {
        get => (Game)GetValue(TheGameProperty);
        set => SetValue(TheGameProperty, value);
    }

    private static void OnGameChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SauvegardeListItem control && newValue is Game game)
        {
            control.BindingContext = new ListItemSave(game);
        }
    }

    public SauvegardeListItem()
    {
        InitializeComponent();
    }
}
