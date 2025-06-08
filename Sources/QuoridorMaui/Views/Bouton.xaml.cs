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

    public static new readonly BindableProperty WidthRequestProperty =
        BindableProperty.Create(nameof(WidthRequest), typeof(double), typeof(Bouton), 300.0);
    public new double WidthRequest
    {
        get => (double)GetValue(WidthRequestProperty);
        set => SetValue(WidthRequestProperty, value);
    }

    public static new readonly BindableProperty HeightRequestProperty =
        BindableProperty.Create(nameof(HeightRequest), typeof(double), typeof(Bouton), 60.0);
    public new double HeightRequest
    {
        get => (double)GetValue(HeightRequestProperty);
        set => SetValue(HeightRequestProperty, value);
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