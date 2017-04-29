using Microsoft.CodeAnalysis;

namespace CodeMetrics.Parsing.Contracts.Roslyn
{
    public interface ICSharpBranchesVisitor : IBranchesVisitor
    {
        void Visit(SyntaxNode node);
    }
}