using CodeMetrics.Parsing;
using ICSharpCode.NRefactory.CSharp;

namespace CodeMetrics.Calculators
{
    using System.Collections.Generic;
    using System.Linq;

    public class ConditionVisitor : DepthFirstAstVisitor, IBranchesVisitor
    {
        public int BranchesCounter { get; private set; }

        private static IDictionary<string, Expression> declerationsDictionary;

        public ConditionVisitor(IDictionary<string, Expression> dictionary = null)
        {
            declerationsDictionary = dictionary ?? new Dictionary<string, Expression>();
        }

        public override void VisitBinaryOperatorExpression(BinaryOperatorExpression binaryOperatorExpression)
        {
            base.VisitBinaryOperatorExpression(binaryOperatorExpression);

            if (binaryOperatorExpression.Operator == BinaryOperatorType.ConditionalAnd || binaryOperatorExpression.Operator == BinaryOperatorType.ConditionalOr)
            {
                BranchesCounter++;
            }
        }


        public override void VisitUnaryOperatorExpression(UnaryOperatorExpression unaryOperatorExpression)
        {
            base.VisitUnaryOperatorExpression(unaryOperatorExpression);

            if (unaryOperatorExpression.Operator == UnaryOperatorType.Not)
            {
                string expressionText = unaryOperatorExpression.Expression.GetText();
                if (declerationsDictionary.ContainsKey(expressionText))
                {
                    declerationsDictionary[expressionText].AcceptVisitor(this);
                }
            }
        }
    }
}