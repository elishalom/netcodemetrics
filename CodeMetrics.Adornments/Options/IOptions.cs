using System.Drawing;

namespace CodeMetrics.Options
{
    public interface IOptions
    {
        int Threshold { get; set; }

        Color MinimumColor { get; set; }

        Color MaximumColor { get; set; }
    }
}