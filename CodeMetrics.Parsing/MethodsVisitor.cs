using System.Collections.Generic;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace CodeMetrics.Parsing
{
    public class MethodsVisitor : AbstractAstVisitor, IMethodsVisitor
    {
        private readonly List<IMethod> methods;

        public MethodsVisitor()
        {
            methods = new List<IMethod>();
        }

        public IEnumerable<IMethod> Methods
        {
            get { return methods; }
        }

        public override object VisitMethodDeclaration(MethodDeclaration methodDeclaration, object data)
        {
            var visitMethodDeclaration = base.VisitMethodDeclaration(methodDeclaration, data);
            var declarationLocation = methodDeclaration.StartLocation.AsLocation();
            var bodyStartLocation = methodDeclaration.Body.StartLocation.AsLocation();
            var bodyEndLocation = methodDeclaration.Body.EndLocation.AsLocation();
            methods.Add(new Method(declarationLocation, bodyStartLocation, bodyEndLocation));
            return visitMethodDeclaration;
        }
    }
}