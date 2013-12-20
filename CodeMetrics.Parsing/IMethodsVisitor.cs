using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp;

namespace CodeMetrics.Parsing
{
    public interface IMethodsVisitor : IAstVisitor
    {
        IEnumerable<IMethod> Methods { get; }
    }
}