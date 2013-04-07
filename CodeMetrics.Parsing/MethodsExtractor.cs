using System.Collections.Generic;
using System.IO;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;

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
            CompilationUnit compilationUnit;
            using (var parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(fileCode)))
            {
                parser.Parse();
                compilationUnit = parser.CompilationUnit;
            }

            var methodsVisitor = visitorsFactory.CreateMethodsVisitor();
            compilationUnit.AcceptVisitor(methodsVisitor, null);

            return methodsVisitor.Methods;
        }
    }
}
