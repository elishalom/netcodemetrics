using System;
using System.Collections.Generic;
using CodeMetrics.Parsing.Contracts.NRefactory;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.PatternMatching;

namespace CodeMetrics.Parsing.NRefactory
{
    public class AstBranchesVisitor : DepthFirstAstVisitor, IAstBranchesVisitor
    {
        private readonly IAstConditionsVisitorFactory conditionsVisitorFactory;
        private readonly Dictionary<string, Expression> declarationsDictionary = new Dictionary<string, Expression>();

        public AstBranchesVisitor(IAstConditionsVisitorFactory conditionsVisitorFactory)
        {
            if (conditionsVisitorFactory == null) throw new ArgumentNullException(nameof(conditionsVisitorFactory));

            this.conditionsVisitorFactory = conditionsVisitorFactory;
        }

        public int Count { get; private set; }

        public override void VisitBinaryOperatorExpression(BinaryOperatorExpression binaryOperatorExpression)
        {
            base.VisitBinaryOperatorExpression(binaryOperatorExpression);

            if (binaryOperatorExpression.Operator == BinaryOperatorType.NullCoalescing)
            {
                Count += 1;
            }
        }

        public override void VisitConditionalExpression(ConditionalExpression conditionalExpression)
        {
            base.VisitConditionalExpression(conditionalExpression);
            Count += 2;
        }

        public override void VisitDoWhileStatement(DoWhileStatement doLoopStatement)
        {
            base.VisitDoWhileStatement(doLoopStatement);

            Count++;
            var conditionComplexity = GetConditionComplexity(doLoopStatement.Condition);
            Count += conditionComplexity;
        }

        public override void VisitForeachStatement(ForeachStatement foreachStatement)
        {
            base.VisitForeachStatement(foreachStatement);

            Count++;
        }

        public override void VisitForStatement(ForStatement forStatement)
        {
            base.VisitForStatement(forStatement);

            Count++;
            var conditionComplexity = GetConditionComplexity(forStatement.Condition);
            Count += conditionComplexity;
        }

        public override void VisitIfElseStatement(IfElseStatement ifElseStatement)
        {
            base.VisitIfElseStatement(ifElseStatement);

            Count++;

            if (!ifElseStatement.FalseStatement.IsNull)
            {
                Count++;
            }

            var conditionComplexity = GetConditionComplexity(ifElseStatement.Condition);
            Count += conditionComplexity;
        }

        public override void VisitReturnStatement(ReturnStatement returnStatement)
        {
            base.VisitReturnStatement(returnStatement);
            var binaryExpressionExpression = returnStatement.Expression as BinaryOperatorExpression;
            if (binaryExpressionExpression == null) return;

            Count++;
            var conditionComplexity = GetConditionComplexity(binaryExpressionExpression);
            Count += conditionComplexity;
        }

        public override void VisitSwitchSection(SwitchSection switchSection)
        {
            base.VisitSwitchSection(switchSection);

            if (!IsDefaultCase(switchSection))
            {
                Count++;
            }
        }

        public override void VisitTryCatchStatement(TryCatchStatement tryCatchStatement)
        {
            base.VisitTryCatchStatement(tryCatchStatement);
            if (tryCatchStatement.CatchClauses != null)
            {
                Count += tryCatchStatement.CatchClauses.Count;
            }
        }

        public override void VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement)
        {
            base.VisitVariableDeclarationStatement(variableDeclarationStatement);

            if (!IsDeclaringCondition(variableDeclarationStatement)) return;
            InitDeclarationsDictionary(variableDeclarationStatement);
        }

        public override void VisitWhileStatement(WhileStatement whileStatement)
        {
            base.VisitWhileStatement(whileStatement);

            Count++;

            var conditionComplexity = GetConditionComplexity(whileStatement.Condition);
            Count += conditionComplexity;
        }

        private static bool IsDeclaringCondition(INode variableDeclarationStatement)
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
            var firstCaseLabel = switchSection.CaseLabels.FirstOrNullObject();
            return firstCaseLabel.Expression.IsNull;
        }

        private IAstConditionsVisitor CreateConditionsVisitor()
        {
            return conditionsVisitorFactory.Create(declarationsDictionary);
        }

        private int GetConditionComplexity(AstNode condition)
        {
            var conditionsVisitor = CreateConditionsVisitor();
            condition.AcceptVisitor(conditionsVisitor);
            return conditionsVisitor.Count;
        }

        private void InitDeclarationsDictionary(VariableDeclarationStatement variableDeclarationStatement)
        {
            foreach (var variable in variableDeclarationStatement.Variables)
            {
                declarationsDictionary[variable.Name] = variable.Initializer;
            }
        }
    }
}