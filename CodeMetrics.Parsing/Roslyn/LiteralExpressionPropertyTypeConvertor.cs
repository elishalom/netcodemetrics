using System;
using CodeMetrics.Parsing.Contracts;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CodeMetrics.Parsing.Roslyn
{
    public class LiteralExpressionPropertyTypeConvertor : IPropertyConvertor
    {
        private readonly IPropertyDeclarationFactory propertyDeclarationFactory;
        private readonly ILocationFactory locationFactory;
        private readonly IUnderlyingObjectFactory underlyingObjectFactory;

        public LiteralExpressionPropertyTypeConvertor(IPropertyDeclarationFactory propertyDeclarationFactory, ILocationFactory locationFactory, IUnderlyingObjectFactory underlyingObjectFactory)
        {
            if (propertyDeclarationFactory == null) throw new ArgumentNullException(nameof(propertyDeclarationFactory));
            if (locationFactory == null) throw new ArgumentNullException(nameof(locationFactory));
            if (underlyingObjectFactory == null) throw new ArgumentNullException(nameof(underlyingObjectFactory));

            this.propertyDeclarationFactory = propertyDeclarationFactory;
            this.locationFactory = locationFactory;
            this.underlyingObjectFactory = underlyingObjectFactory;
        }

        public PropertyType TargetType => PropertyType.LiteralExpression;

        public ISyntaxNodeDeclaration Convert(PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            var location = propertyDeclarationSyntax.GetLocation();
            var locationLineSpan = location.GetLineSpan();
            var declaration = CreateLocation(locationLineSpan.StartLinePosition);

            return CreateProperty(declaration, propertyDeclarationSyntax);
        }

        private ILocation CreateLocation(LinePosition linePosition)
        {
            return locationFactory.Create(linePosition.Line, linePosition.Character);
        }

        private IPropertyDeclaration CreateProperty(ILocation declaration, PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            var underlyingSyntaxNode = underlyingObjectFactory.Create(propertyDeclarationSyntax);
            return propertyDeclarationFactory.Create(declaration, underlyingSyntaxNode);
        }
    }
}