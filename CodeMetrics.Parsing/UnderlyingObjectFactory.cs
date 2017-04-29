using CodeMetrics.Parsing.Contracts;

namespace CodeMetrics.Parsing
{
    public class UnderlyingObjectFactory : IUnderlyingObjectFactory
    {
        public IUnderlyingObject Create(object rawObject)
        {
            return new UnderlyingObject(rawObject);
        }
    }
}