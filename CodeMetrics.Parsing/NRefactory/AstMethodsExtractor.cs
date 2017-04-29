using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeMetrics.Parsing.Contracts;
using CodeMetrics.Parsing.Contracts.NRefactory;
using ICSharpCode.NRefactory.CSharp;

namespace CodeMetrics.Parsing.NRefactory
{
    public class AstMethodsExtractor : IAstMethodsExtractor
    {
        private readonly IAstMethodsVisitorFactory methodsVisitorFactory;
        private readonly IExceptionHandler exceptionHandler;

        public AstMethodsExtractor(IAstMethodsVisitorFactory methodsVisitorFactory, IExceptionHandler exceptionHandler)
        {
            if (methodsVisitorFactory == null) throw new ArgumentNullException(nameof(methodsVisitorFactory));
            if (exceptionHandler == null) throw new ArgumentNullException(nameof(exceptionHandler));

            this.methodsVisitorFactory = methodsVisitorFactory;
            this.exceptionHandler = exceptionHandler;
        }

        public IEnumerable<ISyntaxNodeDeclaration> Extract(string fileCode)
        {
            var methodsVisitor = methodsVisitorFactory.Create();
            try
            {
                AcceptVisitor(fileCode, methodsVisitor);
                return methodsVisitor.Methods;
            }
            catch (InvalidCastException invalidCastException)
            {
                exceptionHandler.HandleException(invalidCastException);
            }
            catch (Exception ex)
            {
                exceptionHandler.HandleException(ex);
            }
            return Enumerable.Empty<ISyntaxNodeDeclaration>();
        }

        private static void AcceptVisitor(string fileCode, IAstVisitor methodsVisitor)
        {
            var syntaxTree = CreateSyntaxTree(fileCode);
            syntaxTree.AcceptVisitor(methodsVisitor);
        }

        private static SyntaxTree CreateSyntaxTree(string fileCode)
        {
            var stringReader = new StringReader(fileCode);
            var parser = new CSharpParser();
            return parser.Parse(stringReader);
        }
    }
}