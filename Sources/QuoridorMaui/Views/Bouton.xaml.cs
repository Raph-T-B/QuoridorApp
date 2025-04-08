



namespace QuoridorMaui.Views;

public partial class Bouton : ContentView
{
    public static readonly BindableProperty TexteProperty = BindableProperty.Create(nameof(Texte), typeof(string), typeof(Bouton), string.Empty);

    public string Texte
    {
        get => (string)GetValue(TexteProperty);
        set => SetValue(TexteProperty, value);
    }

    public Bouton()
    {
        InitializeComponent();
    }


}