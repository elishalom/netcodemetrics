using System.Collections.Generic;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;

namespace CodeMetrics.Parsing
{
    public interface IMethodsVisitor : IAstVisitor
    {
        IEnumerable<IMethod> Methods { get; }
    }

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
            var startLocation = methodDeclaration.StartLocation.AsLocation();
            var endLocation = methodDeclaration.Body.EndLocation.AsLocation();
            methods.Add(new Method(startLocation, endLocation));
            return visitMethodDeclaration;
        }
    }
}