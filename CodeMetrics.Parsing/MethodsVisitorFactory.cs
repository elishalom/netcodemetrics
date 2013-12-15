using System.Collections.Generic;

namespace CodeMetrics.Parsing
{
    public class MethodsVisitorFactory : IMethodsVisitorFactory
    {
        public IEnumerable<IMethodsVisitor> CreateMethodsVisitor()
        {
            return new List<IMethodsVisitor>()
            {
                new MethodsVisitor(),
                new ConstructorsVisitor()
            };
        }
    }
}