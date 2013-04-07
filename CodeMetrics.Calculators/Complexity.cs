using System.ComponentModel;

namespace CodeMetrics.Calculators
{
    public class Complexity : IComplexity, INotifyPropertyChanged
    {
        private int value;
        public int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public Complexity(int value)
        {
            Value = value;
        }

        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };
    }
}