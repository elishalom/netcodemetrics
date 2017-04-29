using System.Collections.Generic;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Roslyn
{
    public class CSharpConditionsVisitor : CSharpSyntaxWalker, ICSharpConditionsVisitor
    {
        private readonly IDictionary<string, ExpressionSyntax> declarationsDictionary;

        public CSharpConditionsVisitor()
        {
        }

        public CSharpConditionsVisitor(IDictionary<string, ExpressionSyntax> declarationDictionary)
        {
            declarationsDictionary = declarationDictionary;
        }

        public int Count { get; private set; }

        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            base.VisitBinaryExpression(node);

            switch (node.Kind())
            {
                case SyntaxKind.LogicalOrExpression:
                case SyntaxKind.LogicalAndExpression:
                    ++Count;
                    break;

                case SyntaxKind.CoalesceExpression:
                    Count += 1;
                    break;
            }
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            base.VisitIdentifierName(node);

            var identifierName = node.Identifier.Text;
            if (declarationsDictionary.ContainsKey(identifierName))
            {
                var expressionSyntaxNode = declarationsDictionary[identifierName];
                Visit(expressionSyntaxNode);
            }
        }
    }
}