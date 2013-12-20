using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp;

namespace CodeMetrics.Parsing
{
    public class PropertyVisitor : DepthFirstAstVisitor, IMethodsVisitor
    {
        private readonly List<IMethod> properties;

        public PropertyVisitor()
        {
            properties = new List<IMethod>();
        }

        public IEnumerable<IMethod> Methods 
        {
            get
            {
                return properties;
            }
        }

        public override void VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
        {
            base.VisitPropertyDeclaration(propertyDeclaration);

            AddAccessorMethod(propertyDeclaration.Getter);
            AddAccessorMethod(propertyDeclaration.Setter);
        }

        private void AddAccessorMethod(Accessor accessor)
        {
            // getter or setter is missing
            if (accessor.StartLocation.Line <= 0)
                return;

            var accessorStartLocation = accessor.StartLocation.AsLocation();
            var accessorEndLocation = accessor.EndLocation.AsLocation();
            properties.Add(new Method(accessorStartLocation, accessorStartLocation, accessorEndLocation));
        }
    }
}