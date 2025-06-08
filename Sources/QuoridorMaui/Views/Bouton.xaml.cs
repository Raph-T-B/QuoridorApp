namespace QuoridorMaui.Views;

public partial class Bouton : ContentView
{
    public static readonly BindableProperty TexteProperty = BindableProperty.Create(nameof(Texte), typeof(string), typeof(Bouton), string.Empty);

    public string Texte
    {
        get => (string)GetValue(TexteProperty);
        set => SetValue(TexteProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(Bouton), 35.0);
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty MinimumWidthRequestProperty =
        BindableProperty.Create(nameof(MinimumWidthRequest), typeof(double), typeof(Bouton), 120.0);
    public double MinimumWidthRequest
    {
        get => (double)GetValue(MinimumWidthRequestProperty);
        set => SetValue(MinimumWidthRequestProperty, value);
    }

    public static readonly BindableProperty MinimumHeightRequestProperty =
        BindableProperty.Create(nameof(MinimumHeightRequest), typeof(double), typeof(Bouton), 40.0);
    public double MinimumHeightRequest
    {
        get => (double)GetValue(MinimumHeightRequestProperty);
        set => SetValue(MinimumHeightRequestProperty, value);
    }

    public event EventHandler Clicked;

    public Bouton()
    {
        InitializeComponent();
    }

    private void OnButtonClicked(object sender, EventArgs e)
    {
        Clicked?.Invoke(this, e);
    }
}