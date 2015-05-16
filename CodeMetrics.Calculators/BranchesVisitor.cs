using System.Collections.Generic;
using System.Linq;
using CodeMetrics.Parsing;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.PatternMatching;

namespace CodeMetrics.Calculators
{


    public class BranchesVisitor : DepthFirstAstVisitor, IBranchesVisitor
    {
        public int BranchesCounter { get; protected set; }

        public override void VisitTryCatchStatement(TryCatchStatement tryCatchStatement)
        {
            base.VisitTryCatchStatement(tryCatchStatement);
            BranchesCounter++;
        }

        public override void VisitConditionalExpression(ConditionalExpression conditionalExpression)
        {
            base.VisitConditionalExpression(conditionalExpression);
            BranchesCounter += 2;
        }

        public override void VisitIfElseStatement(IfElseStatement ifElseStatement)
        {
            base.VisitIfElseStatement(ifElseStatement);

            BranchesCounter++;

            if (!ifElseStatement.FalseStatement.IsNull)
            {
                BranchesCounter++;
            }

            var conditionComplexity = GetConditionComplexity(ifElseStatement.Condition);
            BranchesCounter += conditionComplexity;
        }

        private int GetConditionComplexity(Expression condition)
        {
            var branchesVisitorImpl = new ConditionVisitor();
            string conditionText = condition.GetText();

            if (condition is BinaryOperatorExpression)
            {
                condition.AcceptVisitor(branchesVisitorImpl);
            }
            else if(booleanVariables.Keys.Contains(conditionText))
            {
                booleanVariables[conditionText].AcceptVisitor(branchesVisitorImpl);
            }

            return branchesVisitorImpl.BranchesCounter;
        }

        public override void VisitForStatement(ForStatement forStatement)
        {
            base.VisitForStatement(forStatement);

            BranchesCounter++;
            var conditionComplexity = GetConditionComplexity(forStatement.Condition);
            BranchesCounter += conditionComplexity;
            
        }

        public override void VisitForeachStatement(ForeachStatement foreachStatement)
        {
            base.VisitForeachStatement(foreachStatement);

            BranchesCounter++;
        }


        public override void VisitWhileStatement(WhileStatement whileStatement)
        {
            base.VisitWhileStatement(whileStatement);

            BranchesCounter++;

            var conditionComplexity = GetConditionComplexity(whileStatement.Condition);
            BranchesCounter += conditionComplexity;
        }

        public override void VisitDoWhileStatement(DoWhileStatement doLoopStatement)
        {
            base.VisitDoWhileStatement(doLoopStatement);

            BranchesCounter++;
            var conditionComplexity = GetConditionComplexity(doLoopStatement.Condition);
            BranchesCounter += conditionComplexity;
        }

        public override void VisitSwitchSection(SwitchSection switchSection)
        {
            base.VisitSwitchSection(switchSection);

            if (!IsDefaultCase(switchSection))
            {
                BranchesCounter++;
            }
        }

        private readonly Dictionary<string, Expression> booleanVariables = new Dictionary<string, Expression>();

        public override void VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement)
        {
            base.VisitVariableDeclarationStatement(variableDeclarationStatement);

            if (isDeclaringCondition(variableDeclarationStatement))
            {
                foreach (var variable in variableDeclarationStatement.Variables)
                {
                    booleanVariables[variable.Name] = variable.Initializer;
                }
            }
        }

        private static bool isDeclaringCondition(VariableDeclarationStatement variableDeclarationStatement)
        {
            var conditionalAndPattern = new VariableDeclarationStatement
                    {
                        Type = new AnyNode(),
                        Variables = 
                        { 
                            new VariableInitializer
                                {
                                    Name = Pattern.AnyString,
                                    Initializer = new BinaryOperatorExpression
                                            {
                                                Left = new AnyNode(),
                                                Operator = BinaryOperatorType.ConditionalAnd,
                                                Right = new AnyNode()
                                            }
                                }
                        }
                    };

            var conditionalOrPattern = new VariableDeclarationStatement
                    {
                        Type = new AnyNode(),
                        Variables = 
                        { 
                            new VariableInitializer
                                {
                                    Name = Pattern.AnyString,
                                    Initializer = new BinaryOperatorExpression
                                            {
                                                Left = new AnyNode(),
                                                Operator = BinaryOperatorType.ConditionalOr,
                                                Right = new AnyNode()
                                            }
                                }
                        }
                    };

            return conditionalAndPattern.IsMatch(variableDeclarationStatement) ||
                   conditionalOrPattern.IsMatch(variableDeclarationStatement);
        }

        private static bool IsDefaultCase(SwitchSection switchSection)
        {
            CaseLabel firstCaseLabel = switchSection.CaseLabels.FirstOrNullObject();
            return firstCaseLabel.Expression.IsNull;
        }
    }
}