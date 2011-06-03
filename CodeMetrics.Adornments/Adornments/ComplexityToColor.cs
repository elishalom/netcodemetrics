using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CodeMetrics.Adornments
{
    public class ComplexityToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return Brushes.Transparent;
            }

            var complexity = (int)value;
            var ratio = (Math.Min(complexity, 10))/10D;
            
            var goodColor = Brushes.Green.Color;
            var badColor = Brushes.Red.Color;

            Color combinedColor = CombineColor(badColor, ratio, goodColor);

            return new SolidColorBrush(combinedColor);

        }

        private static Color CombineColor(Color color2, double ratio, Color color1)
        {
            return Color.FromRgb((byte)(color2.R*ratio + color1.R*(1D - ratio)),
                                 (byte)(color2.G*ratio + color1.G*(1D - ratio)),
                                 (byte)(color2.B*ratio + color1.B*(1D - ratio)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}