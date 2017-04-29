namespace CodeMetrics.Calculators.Contracts.NRefactory
{
    public interface IAstCyclomaticComplexityCalculatorFactory
    {
        IAstCyclomaticComplexityCalculator Create();
    }
}