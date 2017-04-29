using System.Linq;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Roslyn
{
    public class LiteralExpressionPropertyTypeResolvingRule : IPropertyTypeResolvingRule
    {
        public bool IsSuitable(PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            return propertyDeclarationSyntax.DescendantNodes().OfType<LiteralExpressionSyntax>().Any();
        }

        public PropertyType PropertyType => PropertyType.LiteralExpression;
    }
}