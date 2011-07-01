using CodeMetrics.Parsing;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace CodeMetrics.Calculators
{
    public class ConditionVisitor : AbstractAstVisitor, IBranchesVisitor
    {
        public int BranchesCounter { get; private set; }

        public override object VisitBinaryOperatorExpression(BinaryOperatorExpression binaryOperatorExpression, object data)
        {
            if (binaryOperatorExpression.Op == BinaryOperatorType.LogicalAnd || binaryOperatorExpression.Op == BinaryOperatorType.LogicalOr)
            {
                BranchesCounter++;
            }
            return base.VisitBinaryOperatorExpression(binaryOperatorExpression, data);
        }
    }
}