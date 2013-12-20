using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp;

namespace CodeMetrics.Parsing
{
    public class ConstructorsVisitor : DepthFirstAstVisitor, IMethodsVisitor
    {
        private readonly List<IMethod> constructors;

        public ConstructorsVisitor()
        {
            constructors = new List<IMethod>();
        }

        public IEnumerable<IMethod> Methods
        {
            get { return constructors; }
        }

        public override void VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration)
        {
            base.VisitConstructorDeclaration(constructorDeclaration);

            var declarationLocation = constructorDeclaration.StartLocation.AsLocation();
            var bodyStartLocation = constructorDeclaration.Body.StartLocation.AsLocation();
            var bodyEndLocation = constructorDeclaration.Body.EndLocation.AsLocation();
            constructors.Add(new Method(declarationLocation, bodyStartLocation, bodyEndLocation));
        }
    }
}