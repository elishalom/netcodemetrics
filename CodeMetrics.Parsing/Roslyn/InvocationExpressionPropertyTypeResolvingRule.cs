using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Roslyn
{
    public class InvocationExpressionPropertyTypeResolvingRule : IPropertyTypeResolvingRule
    {
        public bool IsSuitable(PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            return propertyDeclarationSyntax.ExpressionBody != null;
        }

        public PropertyType PropertyType => PropertyType.InvocationExpression;
    }
}