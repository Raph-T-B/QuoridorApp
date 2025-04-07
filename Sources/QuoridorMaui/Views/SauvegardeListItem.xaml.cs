namespace QuoridorMaui.Views;

public partial class SauvegardeListItem : ContentView
{
    public static readonly BindableProperty PartieBOGlobalProperty = BindableProperty.Create(nameof(PartieBOGlobal), typeof(string), typeof(SauvegardeListItem), string.Empty);
    public string PartieBOGlobal
    {
        get => (string)GetValue(SauvegardeListItem.PartieBOGlobalProperty);
        set => SetValue(SauvegardeListItem.PartieBOGlobalProperty, value);
    }
    public static readonly BindableProperty PartieBO1Property = BindableProperty.Create(nameof(PartieBO1), typeof(string), typeof(SauvegardeListItem), string.Empty);
    public string PartieBO1
    {
        get => (string)GetValue(SauvegardeListItem.PartieBO1Property);
        set => SetValue(SauvegardeListItem.PartieBO1Property, value);
    }

    public static readonly BindableProperty PartieBO2Property = BindableProperty.Create(nameof(PartieBO2), typeof(string), typeof(SauvegardeListItem), string.Empty);
    public string PartieBO2
    {
        get => (string)GetValue(SauvegardeListItem.PartieBO2Property);
        set => SetValue(SauvegardeListItem.PartieBO2Property, value);
    }

    public static readonly BindableProperty PartiePlayer1Property = BindableProperty.Create(nameof(PartiePlayer1), typeof(string), typeof(SauvegardeListItem), string.Empty);
    public string PartiePlayer1
    {
        get => (string)GetValue(SauvegardeListItem.PartiePlayer1Property);
        set => SetValue(SauvegardeListItem.PartiePlayer1Property, value);
    }

    public static readonly BindableProperty PartiePlayer2Property = BindableProperty.Create(nameof(PartiePlayer2), typeof(string), typeof(SauvegardeListItem), string.Empty);
    public string PartiePlayer2
    {
        get => (string)GetValue(SauvegardeListItem.PartiePlayer2Property);
        set => SetValue(SauvegardeListItem.PartiePlayer2Property, value);
    }
    public SauvegardeListItem()
	{
		InitializeComponent();
	}
}