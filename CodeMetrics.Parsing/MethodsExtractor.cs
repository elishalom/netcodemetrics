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

            IMethodsVisitor methodsVisitor = visitorsFactory.CreateMethodsVisitor();
            syntaxTree.AcceptVisitor(methodsVisitor);
            return methodsVisitor.Methods;
        }
    }
}
