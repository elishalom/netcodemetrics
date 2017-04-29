using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp;

namespace CodeMetrics.Parsing.Contracts.NRefactory
{
    public interface IAstConditionsVisitorFactory
    {
        IAstConditionsVisitor Create();
        IAstConditionsVisitor Create(Dictionary<string, Expression> declarationDictionary);
    }
}