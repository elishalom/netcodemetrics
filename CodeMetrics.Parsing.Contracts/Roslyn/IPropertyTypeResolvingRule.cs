using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Contracts.Roslyn
{
    public interface IPropertyTypeResolvingRule
    {
        bool IsSuitable(PropertyDeclarationSyntax propertyDeclarationSyntax);

        PropertyType PropertyType { get; }
    }
}