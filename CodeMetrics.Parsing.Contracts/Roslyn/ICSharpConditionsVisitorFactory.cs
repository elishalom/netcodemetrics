using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Contracts.Roslyn
{
    public interface ICSharpConditionsVisitorFactory
    {
        ICSharpConditionsVisitor Create();

        ICSharpConditionsVisitor Create(IDictionary<string, ExpressionSyntax> declarationDictionary);
    }
}