using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Contracts.Roslyn
{
    public interface IMethodTypeResolvingRule
    {
        bool IsSuitable(MethodDeclarationSyntax declarationSyntax);

        MethodType MethodType { get; }
    }
}