using Microsoft.CodeAnalysis;

namespace CodeMetrics.Parsing.Contracts.Roslyn
{
    public interface ICSharpConditionsVisitor : IBranchesVisitor
    {
        void Visit(SyntaxNode node);
    }
}