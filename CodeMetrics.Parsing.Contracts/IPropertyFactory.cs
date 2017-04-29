namespace CodeMetrics.Parsing.Contracts
{
    public interface IPropertyFactory
    {
        IProperty Create(ILocation declaration, ILocation bodyStart, ILocation bodyEnd, IUnderlyingObject underlyingSyntaxNode);
    }
}