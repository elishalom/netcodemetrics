using CodeMetrics.Parsing;

namespace CodeMetrics.Calculators
{
    public interface IMethodsVisitorFactory
    {
        IMethodsVisitor Create();
    }
}