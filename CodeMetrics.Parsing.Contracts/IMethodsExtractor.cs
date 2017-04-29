using System.Collections.Generic;

namespace CodeMetrics.Parsing.Contracts
{
    public interface IMethodsExtractor
    {
        IEnumerable<ISyntaxNodeDeclaration> Extract(string fileCode);
    }
}