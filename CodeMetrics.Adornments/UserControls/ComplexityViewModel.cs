using System.ComponentModel;
using CodeMetrics.Calculators;

namespace CodeMetrics.UserControls
{
    internal class ComplexityViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IComplexity complexity;

        public int Value { get { return this.complexity == null ? 0 : this.complexity.Value; } }

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
