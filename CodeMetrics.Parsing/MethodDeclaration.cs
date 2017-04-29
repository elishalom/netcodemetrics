using CodeMetrics.Parsing.Contracts;

namespace CodeMetrics.Parsing
{
    public class MethodDeclaration : SyntaxNodeDeclaration, IMethodDeclaration
    {
        public MethodDeclaration(ILocation declaration, IUnderlyingObject underlyingSyntaxNode)
            : base(declaration, underlyingSyntaxNode)
        {
        }
    }
}