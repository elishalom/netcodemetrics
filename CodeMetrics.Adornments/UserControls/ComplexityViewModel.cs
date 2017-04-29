using System.ComponentModel;
using System.Windows.Media;
using CodeMetrics.Adornments;
using CodeMetrics.Calculators.Contracts;
using CodeMetrics.Options;

namespace CodeMetrics.UserControls
{
    internal class ComplexityViewModel : INotifyPropertyChanged
    {
        private readonly IOptions options;

        public event PropertyChangedEventHandler PropertyChanged;

        private ICyclomaticComplexity complexity;

        private readonly ComplexityToColor converter;

        public int Value => complexity?.Value ?? 0;

        public Color Color => this.converter.Convert(this.Value);

        public bool Visible => this.options.MinimumToShow <= Value;

        public ComplexityViewModel(IOptions options)
        {
            this.options = options;
            this.converter = new ComplexityToColor(options);
        }

        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public void UpdateComplexity(ICyclomaticComplexity complexity)
        {
            this.complexity = complexity;
            InvokePropertyChanged(new PropertyChangedEventArgs(null));
        }
    }
}
