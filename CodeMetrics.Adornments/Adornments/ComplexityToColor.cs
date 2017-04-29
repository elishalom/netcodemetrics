using System;
using System.Windows.Media;
using CodeMetrics.Options;

namespace CodeMetrics.Adornments
{
    internal class ComplexityToColor
    {
        private readonly IOptions options;

        private const int MaximumComplexityThreshold = 10;

        public ComplexityToColor(IOptions options)
        {
            this.options = options;
        }

        public Color Convert(int complexity)
        {
            var ratio = (Math.Min(complexity, MaximumComplexityThreshold)) / (double)MaximumComplexityThreshold;
            var maximumColor = ToMediaColor(this.options.BadColor);
            var minimumColor = ToMediaColor(this.options.GoodColor);
            return CombineColor(maximumColor, ratio, minimumColor);
        }

        private static Color ToMediaColor(System.Drawing.Color source)
        {
            return Color.FromArgb(source.A, source.R, source.G, source.B);
        }

        private static Color CombineColor(Color color2, double ratio, Color color1)
        {
            return Color.FromRgb((byte)(color2.R * ratio + color1.R * (1D - ratio)),
                                 (byte)(color2.G * ratio + color1.G * (1D - ratio)),
                                 (byte)(color2.B * ratio + color1.B * (1D - ratio)));
        }
    }
}