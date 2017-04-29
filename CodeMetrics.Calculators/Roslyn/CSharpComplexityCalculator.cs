using System;
using System.Collections.Generic;
using CodeMetrics.Calculators.Contracts;
using CodeMetrics.Parsing.Contracts;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeMetrics.Calculators.Roslyn
{
    public class CSharpComplexityCalculator : ICyclomaticComplexityCalculator
    {
        private readonly ICSharpBranchesVisitorFactory branchesVisitorFactory;
        private readonly ICyclomaticComplexityFactory cyclomaticComplexityFactory;
        private readonly IExceptionHandler exceptionHandler;

        public CSharpComplexityCalculator(ICSharpBranchesVisitorFactory branchesVisitorFactory, ICyclomaticComplexityFactory cyclomaticComplexityFactory, IExceptionHandler exceptionHandler)
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

        private static void Visit(IEnumerable<SyntaxNode> blockStatements, ICSharpBranchesVisitor branchesVisitor)
        {
            foreach (var statement in blockStatements)
            {
                branchesVisitor.Visit(statement);
            }
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

        private static IEnumerable<SyntaxNode> ParseStatements(ISyntaxNodeDeclaration syntaxNode)
        {
            var syntaxNodes = (CSharpSyntaxNode)syntaxNode.UnderlyingSyntaxNode.RawObject;
            var result = new List<CSharpSyntaxNode> { syntaxNodes };
            return result;
        }

        private ICyclomaticComplexity TryCalculate(ISyntaxNodeDeclaration syntaxNode)
        {
            var blockStatements = ParseStatements(syntaxNode);
            var branchesVisitor = branchesVisitorFactory.Create();
            Visit(blockStatements, branchesVisitor);
            return CreateComplexity(branchesVisitor);
        }
    }
}