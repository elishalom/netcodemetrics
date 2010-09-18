using CodeMetrics.Calculators;

namespace CodeMetrics.Addin
{
    public partial class ComplexityView
    {
        public ComplexityView(IComplexity complexity)
        {
            InitializeComponent();
            DataContext = complexity;
        }
    }
}