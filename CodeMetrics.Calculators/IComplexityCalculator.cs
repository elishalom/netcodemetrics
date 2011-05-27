namespace CodeMetrics.Calculators
{
    public interface IComplexityCalculator
    {
        IComplexity Calculate(string method);
    }
}