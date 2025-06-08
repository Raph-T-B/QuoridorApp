using System.Globalization;

namespace QuoridorMaui.Converters;

public class BoolToFontSizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isPawn)
        {
            return isPawn ? 60.0 : 20.0;
        }
        return 20.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 