using CodeMetrics.Parsing;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace CodeMetrics.Calculators
{
    public class BranchesVisitor : AbstractAstVisitor, IBranchesVisitor
    {
        public int BranchesCounter { get; protected set; }

        public override object VisitIfElseStatement(IfElseStatement ifElseStatement, object data)
        {
            BranchesCounter++;

            if (ifElseStatement.HasElseStatements)
            {
                BranchesCounter++;
            }

            var conditionComplexity = GetConditionComplexity(ifElseStatement.Condition);
            BranchesCounter += conditionComplexity;

            return base.VisitIfElseStatement(ifElseStatement, data);
        }

        private static int GetConditionComplexity(Expression condition)
        {
            var branchesVisitorImpl = new ConditionVisitor();
            condition.AcceptVisitor(branchesVisitorImpl, null);
            return branchesVisitorImpl.BranchesCounter;
        }

        public override object VisitForStatement(ForStatement forStatement, object data)
        {
            BranchesCounter++;
            var conditionComplexity = GetConditionComplexity(forStatement.Condition);
            BranchesCounter += conditionComplexity;
            return base.VisitForStatement(forStatement, data);
        }

        public override object VisitForeachStatement(ForeachStatement foreachStatement, object data)
        {
            BranchesCounter++;
            return base.VisitForeachStatement(foreachStatement, data);
        }

        public override object VisitDoLoopStatement(DoLoopStatement doLoopStatement, object data)
        {
            BranchesCounter++;
            var conditionComplexity = GetConditionComplexity(doLoopStatement.Condition);
            BranchesCounter += conditionComplexity;
            return base.VisitDoLoopStatement(doLoopStatement, data);
        }
    }
}