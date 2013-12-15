using System.Collections.Generic;
using System.IO;
using ICSharpCode.NRefactory.CSharp;

namespace CodeMetrics.Parsing
{
    public class MethodsExtractor : IMethodsExtractor
    {
        private readonly IMethodsVisitorFactory visitorsFactory;

        public MethodsExtractor(IMethodsVisitorFactory visitorsFactory)
        {
            this.visitorsFactory = visitorsFactory;
        }

        public IEnumerable<IMethod> Extract(string fileCode)
        {
            var parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(new StringReader(fileCode));

            var methods = new List<IMethod>();
            var methodsVisitor = visitorsFactory.CreateMethodsVisitor();
            foreach (var visitor in methodsVisitor)
            {
                syntaxTree.AcceptVisitor(visitor);
                methods.AddRange(visitor.Methods);
            }

            return methods;
        }
    }
}
