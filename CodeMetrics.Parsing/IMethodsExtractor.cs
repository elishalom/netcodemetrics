using System.Collections.Generic;

namespace CodeMetrics.Parsing
{
    public interface IMethodsExtractor
    {
        IEnumerable<IMethod> Extract(string fileCode);
    }
}