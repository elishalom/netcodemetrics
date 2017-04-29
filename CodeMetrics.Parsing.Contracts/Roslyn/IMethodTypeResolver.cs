using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Contracts.Roslyn
{
    public interface IMethodTypeResolver
    {
        MethodType GetMethodType(MethodDeclarationSyntax declarationSyntax);
    }
}