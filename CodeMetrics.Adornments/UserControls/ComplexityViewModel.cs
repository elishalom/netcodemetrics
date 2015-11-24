using System.ComponentModel;
using CodeMetrics.Calculators;

namespace CodeMetrics.UserControls
{
    internal class ComplexityViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private IComplexity complexity;

        public void UpdateComplexity(IComplexity complexity)
        {
            this.complexity = complexity;
            InvokePropertyChanged(new PropertyChangedEventArgs(null));
        }

        public int Value { get { return complexity == null ? 0 : complexity.Value; } }
        
    }
}
