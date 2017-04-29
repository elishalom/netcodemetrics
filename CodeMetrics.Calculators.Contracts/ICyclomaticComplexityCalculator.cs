using CodeMetrics.Parsing.Contracts;

namespace CodeMetrics.Calculators.Contracts
{
    public interface ICyclomaticComplexityCalculator
    {
        ICyclomaticComplexity Calculate(ISyntaxNode syntaxNode);
    }
}