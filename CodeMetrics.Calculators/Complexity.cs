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

        public override string ToString()
        {
            return string.Format("Complexity:{0}", value);
        }
    }
}