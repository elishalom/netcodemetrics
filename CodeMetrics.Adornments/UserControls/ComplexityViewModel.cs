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
        private readonly IOptions options;

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

        public bool Visible
        {
            get
            {
                return this.options.MinimumToShow <= Value;
            }
        }

        public ComplexityViewModel(IOptions options)
        {
            this.options = options;
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
