using System.Linq;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Roslyn
{
    public class DeclarationMethodTypeResolvingRule : IMethodTypeResolvingRule
    {
        public MethodType MethodType => MethodType.Declaration;

        public bool IsSuitable(MethodDeclarationSyntax declarationSyntax)
        {
            return IsInterfaceDeclarationSyntax(declarationSyntax)
                || IsAbstractMethodDefinition(declarationSyntax)
                || IsEmptyMethodWithoutBody(declarationSyntax);
        }

        private static bool IsAbstractMethodDefinition(Microsoft.CodeAnalysis.SyntaxNode declarationSyntax)
        {
            var childTokens = declarationSyntax.ChildTokens();
            return childTokens.Any(t => t.Kind() == SyntaxKind.AbstractKeyword);
        }

        private static bool IsInterfaceDeclarationSyntax(Microsoft.CodeAnalysis.SyntaxNode declarationSyntax)
        {
            return declarationSyntax.Parent is InterfaceDeclarationSyntax;
        }

        private static bool IsEmptyMethodWithoutBody(MethodDeclarationSyntax declarationSyntax)
        {
            return declarationSyntax.Body == null &&
                declarationSyntax.ExpressionBody == null;
        }
    }
}