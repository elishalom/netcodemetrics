using System;
using System.Windows.Media;

namespace CodeMetrics.Adornments
{
    public class ComplexityToColor
    {
        private const int MaximumComplexityThreshold = 10;

        public Color Convert(int complexity)
        {
            var ratio = (Math.Min(complexity, MaximumComplexityThreshold))/ (double)MaximumComplexityThreshold;
            var goodColor = Brushes.Green.Color;
            var badColor = Brushes.Red.Color;
            return CombineColor(badColor, ratio, goodColor);
        }

        private static Color CombineColor(Color color2, double ratio, Color color1)
        {
            return Color.FromRgb((byte)(color2.R*ratio + color1.R*(1D - ratio)),
                                 (byte)(color2.G*ratio + color1.G*(1D - ratio)),
                                 (byte)(color2.B*ratio + color1.B*(1D - ratio)));
        }
    }
}