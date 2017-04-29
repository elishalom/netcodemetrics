using CodeMetrics.Parsing.Contracts;

namespace CodeMetrics.Parsing
{
    public class Constructor : SyntaxNode, IConstructor
    {
        public Constructor(ILocation declaration, ILocation bodyStart, ILocation bodyEnd, IUnderlyingObject underlyingSyntaxNode)
            : base(declaration, bodyStart, bodyEnd, underlyingSyntaxNode)
        {
        }

        public override string ToString()
        {
            var declaration = Declaration.ToShortString();
            var bodyStart = BodyStart.ToShortString();
            var bodyEnd = BodyEnd.ToShortString();
            return $"Constructor:[{declaration}-{bodyStart}-{bodyEnd}]";
        }
    }
}