namespace CodeMetrics.Calculators.Contracts
{
    public interface ICyclomaticComplexityFactory
    {
        ICyclomaticComplexity Create(int value);
    }
}