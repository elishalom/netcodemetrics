using ICSharpCode.NRefactory;

namespace CodeMetrics.Calculators
{
    public interface IMethodsVisitor : IAstVisitor
    {
        int BranchesCounter { get; }
    }
}