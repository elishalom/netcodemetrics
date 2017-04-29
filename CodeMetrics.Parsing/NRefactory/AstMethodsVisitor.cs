using System;
using System.Collections.Generic;
using CodeMetrics.Parsing.Contracts;
using CodeMetrics.Parsing.Contracts.NRefactory;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp;

namespace CodeMetrics.Parsing.NRefactory
{
    public class AstMethodsVisitor : DepthFirstAstVisitor, IAstMethodsVisitor
    {
        private readonly ILocationFactory locationFactory;
        private readonly IUnderlyingObjectFactory underlyingObjectFactory;
        private readonly IMethodFactory methodFactory;
        private readonly List<IMethod> methods = new List<IMethod>();

        public AstMethodsVisitor(IMethodFactory methodFactory, ILocationFactory locationFactory, IUnderlyingObjectFactory underlyingObjectFactory)
        {
            if (methodFactory == null) throw new ArgumentNullException(nameof(methodFactory));
            if (locationFactory == null) throw new ArgumentNullException(nameof(locationFactory));
            if (underlyingObjectFactory == null) throw new ArgumentNullException(nameof(underlyingObjectFactory));

            this.methodFactory = methodFactory;
            this.locationFactory = locationFactory;
            this.underlyingObjectFactory = underlyingObjectFactory;
        }

        public IEnumerable<IMethod> Methods => methods;

        public override void VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration)
        {
            base.VisitConstructorDeclaration(constructorDeclaration);

            AddMethod(constructorDeclaration, constructorDeclaration.Body);
        }

        public override void VisitMethodDeclaration(ICSharpCode.NRefactory.CSharp.MethodDeclaration methodDeclaration)
        {
            base.VisitMethodDeclaration(methodDeclaration);

            AddMethod(methodDeclaration, methodDeclaration.Body);
        }

        public override void VisitPropertyDeclaration(ICSharpCode.NRefactory.CSharp.PropertyDeclaration propertyDeclaration)
        {
            base.VisitPropertyDeclaration(propertyDeclaration);

            AddAccessorMethod(propertyDeclaration.Getter);
            AddAccessorMethod(propertyDeclaration.Setter);
        }

        private void AddAccessorMethod(Accessor accessor)
        {
            AddMethod(accessor, accessor.Body);
        }

        private void AddMethod(AstNode methodOrConstructorDeclaration, AstNode body)
        {
            if (body.IsNull)
            {
                return;
            }

            var declarationLocation = CreateLocation(methodOrConstructorDeclaration.StartLocation);
            var bodyStartLocation = CreateLocation(body.StartLocation);
            var bodyEndLocation = CreateLocation(body.EndLocation);

            var method = CreateMethod(declarationLocation, bodyStartLocation, bodyEndLocation, body);
            methods.Add(method);
        }

        private ILocation CreateLocation(TextLocation textLocation)
        {
            return locationFactory.Create(textLocation.Line - 1, textLocation.Column - 1);
        }

        private IMethod CreateMethod(ILocation declarationLocation, ILocation bodyStartLocation, ILocation bodyEndLocation, object rawObject)
        {
            if (declarationLocation == null) throw new ArgumentNullException(nameof(declarationLocation));
            if (bodyStartLocation == null) throw new ArgumentNullException(nameof(bodyStartLocation));
            if (bodyEndLocation == null) throw new ArgumentNullException(nameof(bodyEndLocation));
            if (rawObject == null) throw new ArgumentNullException(nameof(rawObject));

            var underlyingSyntaxNode = underlyingObjectFactory.Create(rawObject);
            return methodFactory.Create(declarationLocation, bodyStartLocation, bodyEndLocation, underlyingSyntaxNode);
        }
    }
}