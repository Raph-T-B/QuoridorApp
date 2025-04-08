namespace QuoridorMaui.Views;

public partial class RuleView : ContentView
{
    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(RuleView), string.Empty,
            propertyChanged: OnImageSourceChanged);

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(RuleView), string.Empty,
            propertyChanged: OnTitleChanged);

    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(RuleView), string.Empty,
            propertyChanged: OnDescriptionChanged);

    public string ImageSource
    {
        get => (string)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    private static void OnImageSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is RuleView view && newValue is string imageSource)
        {
            view.ruleImage.Source = imageSource;
        }
    }

    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is RuleView view && newValue is string title)
        {
            view.ruleTitle.Text = title;
        }
    }

    private static void OnDescriptionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is RuleView view && newValue is string description)
        {
            view.ruleDescription.Text = description;
        }
    }

    public RuleView()
    {
        InitializeComponent();
    }
} 