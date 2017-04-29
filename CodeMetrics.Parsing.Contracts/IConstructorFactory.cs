namespace CodeMetrics.Parsing.Contracts
{
    public interface IConstructorFactory
    {
        IConstructor Create(ILocation declaration, ILocation bodyStart, ILocation bodyEnd, IUnderlyingObject underlyingSyntaxNode);
    }
}