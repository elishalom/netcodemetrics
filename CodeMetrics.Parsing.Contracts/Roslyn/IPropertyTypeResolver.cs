using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Contracts.Roslyn
{
    public interface IPropertyTypeResolver
    {
        PropertyType GetPropertyType(PropertyDeclarationSyntax propertyDeclarationSyntax);
    }
}