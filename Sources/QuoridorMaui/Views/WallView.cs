using Microsoft.Maui.Controls;
using QuoridorLib.Models;

namespace QuoridorMaui.Views;

public class WallView : BoxView
{
    public static readonly BindableProperty OrientationProperty =
        BindableProperty.Create(nameof(Orientation), typeof(string), typeof(WallView), "horizontal");

    public string Orientation
    {
        get => (string)GetValue(OrientationProperty);
        set
        {
            SetValue(OrientationProperty, value);
            UpdateRotation();
        }
    }

    public WallView()
    {
        Color = Colors.Brown;
        HeightRequest = 20;
        WidthRequest = 100;
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        Rotation = Orientation == "vertical" ? 90 : 0;
    }
} 