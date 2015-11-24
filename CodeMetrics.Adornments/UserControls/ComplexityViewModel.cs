using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;
using CodeMetrics.Adornments;
using CodeMetrics.Calculators;
using CodeMetrics.Options;

namespace CodeMetrics.UserControls
{
    internal class ComplexityViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IComplexity complexity;

        private readonly ComplexityToColor converter;

        public int Value { get { return this.complexity == null ? 0 : this.complexity.Value; } }

        public Color Color
        {
            get
            {
                return this.converter.Convert(this.Value);
            }
        }

        public ComplexityViewModel(IOptions options)
        {
            this.converter = new ComplexityToColor(options);
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
