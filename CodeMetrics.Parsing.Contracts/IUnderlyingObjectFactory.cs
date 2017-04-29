namespace CodeMetrics.Parsing.Contracts
{
    public interface IUnderlyingObjectFactory
    {
        IUnderlyingObject Create(object rawObject);
    }
}