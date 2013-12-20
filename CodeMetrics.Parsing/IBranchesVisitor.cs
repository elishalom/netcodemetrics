using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp;

namespace CodeMetrics.Parsing
{
    public interface IBranchesVisitor : IAstVisitor
    {
        int BranchesCounter { get; }
    }
}