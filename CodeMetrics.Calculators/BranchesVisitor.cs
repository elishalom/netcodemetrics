using CodeMetrics.Parsing;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace CodeMetrics.Calculators
{
    public class BranchesVisitor : AbstractAstVisitor, IBranchesVisitor
    {
        public int BranchesCounter { get; private set; }

        public override object VisitIfElseStatement(IfElseStatement ifElseStatement, object data)
        {
            BranchesCounter++;

            if (ifElseStatement.HasElseStatements)
            {
                BranchesCounter++;
            }

            return base.VisitIfElseStatement(ifElseStatement, data);
        }

        public override object VisitForStatement(ForStatement forStatement, object data)
        {
            BranchesCounter++;
            return base.VisitForStatement(forStatement, data);
        }

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