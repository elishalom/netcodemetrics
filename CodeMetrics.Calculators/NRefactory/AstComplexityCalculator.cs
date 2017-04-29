using System;
using System.Collections.Generic;
using CodeMetrics.Calculators.Contracts;
using CodeMetrics.Calculators.Contracts.NRefactory;
using CodeMetrics.Parsing.Contracts;
using CodeMetrics.Parsing.Contracts.NRefactory;
using ICSharpCode.NRefactory.CSharp;

namespace CodeMetrics.Calculators.NRefactory
{
    public class AstCyclomaticComplexityCalculator : IAstCyclomaticComplexityCalculator
    {
        private readonly IAstBranchesVisitorFactory branchesVisitorFactory;
        private readonly ICyclomaticComplexityFactory cyclomaticComplexityFactory;
        private readonly IExceptionHandler exceptionHandler;

        public AstCyclomaticComplexityCalculator(IAstBranchesVisitorFactory branchesVisitorFactory, ICyclomaticComplexityFactory cyclomaticComplexityFactory, IExceptionHandler exceptionHandler)
        {
            if (branchesVisitorFactory == null) throw new ArgumentNullException(nameof(branchesVisitorFactory));
            if (cyclomaticComplexityFactory == null) throw new ArgumentNullException(nameof(cyclomaticComplexityFactory));
            if (exceptionHandler == null) throw new ArgumentNullException(nameof(exceptionHandler));

            this.branchesVisitorFactory = branchesVisitorFactory;
            this.cyclomaticComplexityFactory = cyclomaticComplexityFactory;
            this.exceptionHandler = exceptionHandler;
        }

        public ICyclomaticComplexity Calculate(ISyntaxNode syntaxNode)
        {
            try
            {
                return TryCalculate(syntaxNode);
            }
            catch (NullReferenceException nullReferenceException)
            {
                exceptionHandler.HandleException(nullReferenceException);
                return CreateComplexity(1);
            }
            catch (Exception ex)
            {
                exceptionHandler.HandleException(ex);
                return CreateComplexity(1);
            }
        }

        private static void AcceptVisitors(IEnumerable<Statement> blockStatements, IAstVisitor branchesVisitor)
        {
            foreach (var statement in blockStatements)
            {
                statement.AcceptVisitor(branchesVisitor);
            }
        }

        private static IEnumerable<Statement> ParseStatements(ISyntaxNodeDeclaration syntaxNode)
        {
            var blockStatement = (BlockStatement)syntaxNode.UnderlyingSyntaxNode.RawObject;
            return blockStatement.Statements;
        }

        private ICyclomaticComplexity CreateComplexity(IBranchesCounter branchesVisitor)
        {
            var complexity = branchesVisitor.Count + 1;
            return cyclomaticComplexityFactory.Create(complexity);
        }

        private ICyclomaticComplexity CreateComplexity(int complexity)
        {
            return cyclomaticComplexityFactory.Create(complexity);
        }

        private ICyclomaticComplexity TryCalculate(ISyntaxNodeDeclaration syntaxNode)
        {
            var blockStatements = ParseStatements(syntaxNode);
            var branchesVisitor = branchesVisitorFactory.Create();
            AcceptVisitors(blockStatements, branchesVisitor);
            return CreateComplexity(branchesVisitor);
        }
    }
}