using System;
using System.IO;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;

namespace CodeMetrics.Calculators
{
    public class ComplexityCalculator : IComplexityCalculator
    {
        private readonly IMethodsVisitorFactory methodsVisitorFactory;

        public ComplexityCalculator(IMethodsVisitorFactory methodsVisitorFactory)
        {
            this.methodsVisitorFactory = methodsVisitorFactory;
        }

        public IComplexity Calculate(string method)
        {
            BlockStatement blockStatement;
            using (var parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(method)))
            {
                try
                {
                    blockStatement = parser.ParseBlock();
                }
                catch (NullReferenceException e)
                {
                    return new Complexity(1);
                }
            }

            var visitor = methodsVisitorFactory.Create();
            blockStatement.AcceptVisitor(visitor, null);


            return new Complexity(visitor.IfsCounter + 1);
        }
    }
}