namespace CodeMetrics.Parsing.Contracts
{
    public interface IMethodFactory
    {
        IMethod Create(ILocation declaration, ILocation bodyStart, ILocation bodyEnd, IUnderlyingObject underlyingSyntaxNode);
    }
}