using System.Collections.Generic;

namespace CodeMetrics.Parsing.Contracts
{
    public interface IMethodsVisitor
    {
        IEnumerable<IMethod> Methods { get; }
    }
}