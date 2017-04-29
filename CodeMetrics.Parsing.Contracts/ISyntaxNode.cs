namespace CodeMetrics.Parsing.Contracts
{
    public interface ISyntaxNode : ISyntaxNodeDeclaration
    {
        ILocation BodyStart { get; }
        ILocation BodyEnd { get; }
    }
}