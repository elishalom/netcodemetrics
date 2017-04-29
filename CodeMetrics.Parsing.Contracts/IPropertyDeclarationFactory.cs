namespace CodeMetrics.Parsing.Contracts
{
    public interface IPropertyDeclarationFactory
    {
        IPropertyDeclaration Create(ILocation declaration, object underlyingSyntaxNode);
    }
}