using CodeMetrics.Parsing.Contracts;

namespace CodeMetrics.Parsing
{
    public abstract class SyntaxNode : SyntaxNodeDeclaration, ISyntaxNode
    {
        protected SyntaxNode(ILocation declaration, ILocation bodyStart, ILocation bodyEnd, IUnderlyingObject underlyingSyntaxNode)
            : base(declaration, underlyingSyntaxNode)
        {
            BodyStart = bodyStart;
            BodyEnd = bodyEnd;
        }

        public ILocation BodyEnd { get; }
        public ILocation BodyStart { get; }
    }
}