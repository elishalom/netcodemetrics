using System.Collections.Generic;

namespace CodeMetrics.Parsing.Contracts.Roslyn
{
    public interface ISyntaxNodeExtractor
    {
        IEnumerable<ISyntaxNodeDeclaration> Extract(string fileCode);
    }
}