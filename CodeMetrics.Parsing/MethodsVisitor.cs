using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp;

namespace CodeMetrics.Parsing
{
    public class MethodsVisitor : DepthFirstAstVisitor, IMethodsVisitor
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

        public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
        {
            base.VisitMethodDeclaration(methodDeclaration);

            var declarationLocation = methodDeclaration.StartLocation.AsLocation();
            var bodyStartLocation = methodDeclaration.Body.StartLocation.AsLocation();
            var bodyEndLocation = methodDeclaration.Body.EndLocation.AsLocation();
            methods.Add(new Method(declarationLocation, bodyStartLocation, bodyEndLocation));
        }
    }
}