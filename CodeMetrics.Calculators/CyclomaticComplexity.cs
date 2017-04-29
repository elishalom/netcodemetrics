using System.ComponentModel;
using CodeMetrics.Calculators.Contracts;

namespace CodeMetrics.Calculators
{
    public class CyclomaticComplexity : ICyclomaticComplexity, INotifyPropertyChanged
    {
        private int value;
        public int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        public CyclomaticComplexity(int value)
        {
            Value = value;
        }

        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };

        public override string ToString()
        {
            return $"Complexity:{value}";
        }
    }
}