using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Contracts.Roslyn
{
    public interface IPropertyConvertor
    {
        ISyntaxNodeDeclaration Convert(PropertyDeclarationSyntax propertyDeclarationSyntax);

        PropertyType TargetType { get; }
    }
}