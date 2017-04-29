using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Roslyn
{
    public class BodyMethodTypeResolvingRule : IMethodTypeResolvingRule
    {
        public MethodType MethodType => MethodType.Body;

        public bool IsSuitable(MethodDeclarationSyntax declarationSyntax)
        {
            return declarationSyntax.Body != null;
        }
    }
}