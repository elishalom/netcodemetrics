using System;
using System.Collections.Generic;
using System.Linq;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Roslyn
{
    public class CSharpBranchesVisitor : CSharpSyntaxWalker, ICSharpBranchesVisitor
    {
        private readonly ICSharpConditionsVisitorFactory conditionsVisitorFactory;
        private readonly Dictionary<string, ExpressionSyntax> declarationsDictionary = new Dictionary<string, ExpressionSyntax>();

        public CSharpBranchesVisitor(ICSharpConditionsVisitorFactory conditionsVisitorFactory)
        {
            if (conditionsVisitorFactory == null) throw new ArgumentNullException(nameof(conditionsVisitorFactory));

            this.conditionsVisitorFactory = conditionsVisitorFactory;
        }

        public int Count { get; private set; }

        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            base.VisitBinaryExpression(node);
            if (node.OperatorToken.Kind() != SyntaxKind.QuestionQuestionToken)
                return;
            Count += 1;
        }

        public override void VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
        {
            base.VisitConditionalAccessExpression(node);
            if (node.OperatorToken.Kind() != SyntaxKind.QuestionToken)
                return;
            Count++;
        }

        public override void VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            base.VisitConditionalExpression(node);
            if (node.QuestionToken.Kind() != SyntaxKind.QuestionToken || node.ColonToken.Kind() != SyntaxKind.ColonToken)
                return;
            Count += 2;
        }

        public override void VisitDoStatement(DoStatementSyntax node)
        {
            base.VisitDoStatement(node);
            Count++;
            var conditionComplexity = GetConditionComplexity(node.Condition);
            Count += conditionComplexity;
        }

        public override void VisitElseClause(ElseClauseSyntax node)
        {
            base.VisitElseClause(node);
            Count++;
        }

        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            base.VisitForEachStatement(node);
            Count++;
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            base.VisitForStatement(node);
            Count++;
            var conditionComplexity = GetConditionComplexity(node.Condition);
            Count += conditionComplexity;
        }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            base.VisitIfStatement(node);
            Count++;

            var conditionComplexity = GetConditionComplexity(node.Condition);
            Count += conditionComplexity;
        }

        public override void VisitInitializerExpression(InitializerExpressionSyntax node)
        {
            base.VisitInitializerExpression(node);
            //var binaryExpressions = node?.DescendantNodes()?.OfType<BinaryExpressionSyntax>();
            //if (binaryExpressions != null && binaryExpressions.Any())
            //{
            //    Count++;
            //    foreach (var binaryExpression in binaryExpressions)
            //    {
            //        var conditionComplexity = GetConditionComplexity(binaryExpression);
            //        Count += conditionComplexity;
            //    }
            //}
        }

        public override void VisitReturnStatement(ReturnStatementSyntax node)
        {
            base.VisitReturnStatement(node);
            var binaryExpressionSyntax = node.Expression as BinaryExpressionSyntax;
            if (binaryExpressionSyntax == null) return;

            Count++;
            var conditionComplexity = GetConditionComplexity(binaryExpressionSyntax);
            Count += conditionComplexity;
        }

        public override void VisitSwitchSection(SwitchSectionSyntax node)
        {
            base.VisitSwitchSection(node);
            if (!IsDefaultCase(node))
            {
                Count++;
            }
        }

        public override void VisitTryStatement(TryStatementSyntax node)
        {
            base.VisitTryStatement(node);
            Count += node.Catches.Count;
        }

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            base.VisitVariableDeclaration(node);

            InitDeclarationsDictionary(node);
        }

        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            base.VisitWhileStatement(node);
            Count++;
            var conditionComplexity = GetConditionComplexity(node.Condition);
            Count += conditionComplexity;
        }

        private static bool IsDefaultCase(SwitchSectionSyntax switchSection)
        {
            var firstCaseLabel = switchSection.Labels.OfType<DefaultSwitchLabelSyntax>().FirstOrDefault();
            return firstCaseLabel != null;
        }

        private ICSharpConditionsVisitor CreateConditionsVisitor()
        {
            return conditionsVisitorFactory.Create(declarationsDictionary);
        }

        private int GetConditionComplexity(Microsoft.CodeAnalysis.SyntaxNode node)
        {
            var conditionsVisitor = CreateConditionsVisitor();
            conditionsVisitor.Visit(node);
            return conditionsVisitor.Count;
        }

        private void InitDeclarationsDictionary(VariableDeclarationSyntax variableDeclaration)
        {
            if (variableDeclaration?.Variables == null) return;

            foreach (var variable in variableDeclaration.Variables)
            {
                var binaryExpressionSyntax = variable.Initializer?.DescendantNodes().OfType<BinaryExpressionSyntax>().FirstOrDefault();
                if (binaryExpressionSyntax != null)
                {
                    declarationsDictionary[variable.Identifier.Text] = binaryExpressionSyntax;
                }
            }
        }
    }
}