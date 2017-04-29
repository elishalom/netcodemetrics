namespace CodeMetrics.Parsing.Contracts
{
    public interface IPropertyAccessorFactory
    {
        IPropertyAccessor Create(ILocation declaration, ILocation bodyStart, ILocation bodyEnd, IUnderlyingObject underlyingSyntaxNode);
    }
}