using System.Linq;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Roslyn
{
    public class DeclarationPropertyTypeResolvingRule : IPropertyTypeResolvingRule
    {
        public PropertyType PropertyType => PropertyType.Declaration;

        public bool IsSuitable(PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            return HasAccessorDeclarationSyntaxAsDescendantNodes(propertyDeclarationSyntax)
                   || IsEmptyProperty(propertyDeclarationSyntax);
        }

        private static bool HasAccessorDeclarationSyntaxAsDescendantNodes(Microsoft.CodeAnalysis.SyntaxNode propertyDeclarationSyntax)
        {
            return propertyDeclarationSyntax.DescendantNodes().OfType<AccessorDeclarationSyntax>().Any();
        }

        private static bool HasNoExpressionBody(PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            return propertyDeclarationSyntax.ExpressionBody == null;
        }

        private static bool HasNoLiteralExpression(Microsoft.CodeAnalysis.SyntaxNode propertyDeclarationSyntax)
        {
            return !propertyDeclarationSyntax.DescendantNodes().OfType<LiteralExpressionSyntax>().Any();
        }

        private static bool IsAccessorListEmpty(BasePropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            if (propertyDeclarationSyntax.AccessorList == null) return true;
            return !propertyDeclarationSyntax.AccessorList.Accessors.Any();
        }

        private static bool IsEmptyProperty(PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            return IsAccessorListEmpty(propertyDeclarationSyntax)
                && HasNoExpressionBody(propertyDeclarationSyntax)
                && HasNoLiteralExpression(propertyDeclarationSyntax);
        }
    }
}