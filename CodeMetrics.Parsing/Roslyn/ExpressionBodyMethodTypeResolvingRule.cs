using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Roslyn
{
    public class ExpressionBodyMethodTypeResolvingRule : IMethodTypeResolvingRule
    {
        public MethodType MethodType => MethodType.ExpressionBody;

        public bool IsSuitable(MethodDeclarationSyntax declarationSyntax)
        {
            return declarationSyntax.ExpressionBody != null;
        }
    }
}