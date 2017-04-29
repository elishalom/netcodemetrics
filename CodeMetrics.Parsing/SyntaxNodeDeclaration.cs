using CodeMetrics.Parsing.Contracts;

namespace CodeMetrics.Parsing
{
    public abstract class SyntaxNodeDeclaration : ISyntaxNodeDeclaration
    {
        protected SyntaxNodeDeclaration(ILocation declaration, IUnderlyingObject underlyingSyntaxNode)
        {
            Declaration = declaration;
            UnderlyingSyntaxNode = underlyingSyntaxNode;
        }

        public ILocation Declaration { get; }

        public IUnderlyingObject UnderlyingSyntaxNode { get; }
    }
}