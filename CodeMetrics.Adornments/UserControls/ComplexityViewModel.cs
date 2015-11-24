using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;
using CodeMetrics.Adornments;
using CodeMetrics.Calculators;

namespace CodeMetrics.UserControls
{
    internal class ComplexityViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IComplexity complexity;

        private readonly ComplexityToColor converter = new ComplexityToColor();

        public int Value { get { return this.complexity == null ? 0 : this.complexity.Value; } }

        public Color Color
        {
            get
            {
                return (Color)converter.Convert(this.Value, typeof(Color), null, CultureInfo.InvariantCulture);
            }
        }
        
        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public void UpdateComplexity(IComplexity complexity)
        {
            this.complexity = complexity;
            InvokePropertyChanged(new PropertyChangedEventArgs(null));
        }
    }
}
