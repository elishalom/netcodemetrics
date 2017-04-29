namespace CodeMetrics.Parsing.Contracts
{
    public interface ISyntaxNodeDeclaration
    {
        ILocation Declaration { get; }
        IUnderlyingObject UnderlyingSyntaxNode { get; }
    }
}