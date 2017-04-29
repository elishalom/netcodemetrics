using CodeMetrics.Parsing.Contracts;

namespace CodeMetrics.Parsing
{
    public class PropertyDeclaration : SyntaxNodeDeclaration, IPropertyDeclaration
    {
        public PropertyDeclaration(ILocation declaration, IUnderlyingObject underlyingSyntaxNode)
            : base(declaration, underlyingSyntaxNode)
        {
        }
    }
}