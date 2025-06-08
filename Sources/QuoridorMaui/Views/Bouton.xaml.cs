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

    public static readonly BindableProperty MaximumWidthRequestProperty =
        BindableProperty.Create(nameof(MaximumWidthRequest), typeof(double), typeof(Bouton), 350.0);
    public double MaximumWidthRequest
    {
        get => (double)GetValue(MaximumWidthRequestProperty);
        set => SetValue(MaximumWidthRequestProperty, value);
    }

    public static readonly BindableProperty MaximumHeightRequestProperty =
        BindableProperty.Create(nameof(MaximumHeightRequest), typeof(double), typeof(Bouton), 90.0);
    public double MaximumHeightRequest
    {
        get => (double)GetValue(MaximumHeightRequestProperty);
        set => SetValue(MaximumHeightRequestProperty, value);
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