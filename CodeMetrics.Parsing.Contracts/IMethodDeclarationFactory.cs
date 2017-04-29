namespace CodeMetrics.Parsing.Contracts
{
    public interface IMethodDeclarationFactory
    {
        IMethodDeclaration Create(ILocation declaration, IUnderlyingObject underlyingSyntaxNode);
    }
}