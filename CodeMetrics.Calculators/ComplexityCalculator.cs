using System;
using System.Collections.Generic;
using CodeMetrics.Parsing;
using ICSharpCode.NRefactory.CSharp;

namespace CodeMetrics.Calculators
{
    public class ComplexityCalculator : IComplexityCalculator
    {
        private readonly IBranchesVisitorFactory methodsVisitorFactory;

        public ComplexityCalculator(IBranchesVisitorFactory methodsVisitorFactory)
        {
            this.methodsVisitorFactory = methodsVisitorFactory;
        }

        public IComplexity Calculate(string method)
        {
            try
            {
                return TryCalculate(method);
            }
            catch (NullReferenceException)
            {
                return new Complexity(1);
            }
        }

        private IComplexity TryCalculate(string method)
        {
            IEnumerable<Statement> blockStatement = ParseStatements(method);
            var visitor = methodsVisitorFactory.CreateBranchesVisitor();
            AcceptVisitors(blockStatement, visitor);
            return new Complexity(visitor.BranchesCounter + 1);
        }

        private static void AcceptVisitors(IEnumerable<Statement> blockStatement, IBranchesVisitor visitor)
        {
            foreach (var statement in blockStatement)
            {
                statement.AcceptVisitor(visitor);
            }
        }

        private static IEnumerable<Statement> ParseStatements(string method)
        {
            var parser = new CSharpParser();
            return parser.ParseStatements(method);
        }
    }
}