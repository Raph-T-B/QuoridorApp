using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace QuoridorMaui.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isMovePossible && parameter is string colors)
            {
                var colorNames = colors.Split(',');
                if (colorNames.Length == 2)
                {
                    var color = isMovePossible ? colorNames[0].Trim() : colorNames[1].Trim();
                    return Color.FromArgb(color);
                }
            }
            return Colors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 