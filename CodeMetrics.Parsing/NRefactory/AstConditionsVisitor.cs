using System.Collections.Generic;
using System.Linq;
using CodeMetrics.Parsing.Contracts.NRefactory;
using ICSharpCode.NRefactory.CSharp;

namespace CodeMetrics.Parsing.NRefactory
{
    public class AstConditionsVisitor : DepthFirstAstVisitor, IAstConditionsVisitor
    {
        private readonly IDictionary<string, Expression> declarationsDictionary;

        public AstConditionsVisitor()
        {
            declarationsDictionary = new Dictionary<string, Expression>();
        }

        public AstConditionsVisitor(IDictionary<string, Expression> declarationDictionary)
        {
            declarationsDictionary = declarationDictionary;
        }

        public int Count { get; private set; }

        public override void VisitBinaryOperatorExpression(BinaryOperatorExpression binaryOperatorExpression)
        {
            base.VisitBinaryOperatorExpression(binaryOperatorExpression);

            if (IsAllowed(binaryOperatorExpression.Operator, BinaryOperatorType.ConditionalAnd,
                BinaryOperatorType.ConditionalOr))
            {
                Count++;
            }
        }

        public override void VisitIdentifier(Identifier identifier)
        {
            base.VisitIdentifier(identifier);

            if (declarationsDictionary.ContainsKey(identifier.Name))
            {
                declarationsDictionary[identifier.Name].AcceptVisitor(this);
            }
        }

        private static bool IsAllowed(BinaryOperatorType operatorType, params BinaryOperatorType[] allowedTypes)
        {
            return allowedTypes.Contains(operatorType);
        }
    }
}