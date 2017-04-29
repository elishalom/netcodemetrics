using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Contracts.Roslyn
{
    public interface IMethodConvertor
    {
        ISyntaxNodeDeclaration Convert(MethodDeclarationSyntax methodDeclarationSyntax);

        MethodType TargetType { get; }
    }
}