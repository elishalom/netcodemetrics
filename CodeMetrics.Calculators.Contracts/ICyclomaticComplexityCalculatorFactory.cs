namespace CodeMetrics.Calculators.Contracts
{
    public interface ICyclomaticComplexityCalculatorFactory
    {
        ICyclomaticComplexityCalculator Create();
    }
}