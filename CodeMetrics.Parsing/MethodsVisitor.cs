using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp;

namespace CodeMetrics.Parsing
{
    public class MethodsVisitor : DepthFirstAstVisitor, IMethodsVisitor
    {
        private readonly List<IMethod> methods = new List<IMethod>();

        public IEnumerable<IMethod> Methods
        {
            get { return methods; }
        }

        public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
        {
            base.VisitMethodDeclaration(methodDeclaration);

            AddMethod(methodDeclaration, methodDeclaration.Body);
        }

        public override void VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration)
        {
            base.VisitConstructorDeclaration(constructorDeclaration);

            AddMethod(constructorDeclaration, constructorDeclaration.Body);
        }

        public override void VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
        {
            base.VisitPropertyDeclaration(propertyDeclaration);

            AddAccessorMethod(propertyDeclaration.Getter);
            AddAccessorMethod(propertyDeclaration.Setter);
        }

        private void AddMethod(EntityDeclaration methodDeclaration, BlockStatement body)
        {
            if (body.IsNull)
            {
                return;
            }

            var declarationLocation = methodDeclaration.StartLocation.AsLocation();
            var bodyStartLocation = body.StartLocation.AsLocation();
            var bodyEndLocation = body.EndLocation.AsLocation();
            methods.Add(new Method(declarationLocation, bodyStartLocation, bodyEndLocation));
        }

        private void AddAccessorMethod(Accessor accessor)
        {
            AddMethod(accessor, accessor.Body);
        }

    }
}