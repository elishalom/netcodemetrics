using System.Drawing;

namespace CodeMetrics.Options
{
    public interface IOptions
    {
        int MinimumToShow { get; set; }

        Color GoodColor { get; set; }

        Color BadColor { get; set; }
    }
}