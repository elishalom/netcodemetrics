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
            IEnumerable<Statement> blockStatement;
            var parser = new CSharpParser();
            try
            {
                blockStatement = parser.ParseStatements(method);
            }
            catch (NullReferenceException e)
            {
                return new Complexity(1);
            }

            var visitor = methodsVisitorFactory.CreateBranchesVisitor();
            foreach (var statement in blockStatement)
            {
                statement.AcceptVisitor(visitor);
            }


            return new Complexity(visitor.BranchesCounter + 1);
        }
    }
}