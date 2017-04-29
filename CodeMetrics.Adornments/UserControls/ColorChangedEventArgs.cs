using System;
using System.Drawing;

namespace CodeMetrics.UserControls
{
    internal class ColorChangedEventArgs : EventArgs
    {
        public Color NewColor { get; set; }
    }
}