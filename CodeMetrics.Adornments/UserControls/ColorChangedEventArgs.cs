using System;
using System.Drawing;

namespace CodeMetrics.Options
{
    internal class ColorChangedEventArgs : EventArgs
    {
        public Color NewColor { get; set; }
    }
}