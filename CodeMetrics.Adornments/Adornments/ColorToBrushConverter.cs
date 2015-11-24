using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CodeMetrics.Adornments
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color)
            {
                Color color = (Color)value;
                return new SolidColorBrush(color);
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
