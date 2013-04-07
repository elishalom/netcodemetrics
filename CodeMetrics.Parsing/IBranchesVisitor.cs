using ICSharpCode.NRefactory;

namespace CodeMetrics.Parsing
{
    public interface IBranchesVisitor : IAstVisitor
    {
        int BranchesCounter { get; }
    }
}