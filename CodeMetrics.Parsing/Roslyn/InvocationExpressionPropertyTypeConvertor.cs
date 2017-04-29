using System;
using CodeMetrics.Parsing.Contracts;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CodeMetrics.Parsing.Roslyn
{
    public class InvocationExpressionPropertyTypeConvertor : IPropertyConvertor
    {
        private readonly ILocationFactory locationFactory;

        private readonly IUnderlyingObjectFactory underlyingObjectFactory;

        private readonly IPropertyFactory propertyFactory;

        public InvocationExpressionPropertyTypeConvertor(
            IPropertyFactory propertyFactory,
            ILocationFactory locationFactory,
            IUnderlyingObjectFactory underlyingObjectFactory)
        {
            if (propertyFactory == null) throw new ArgumentNullException(nameof(propertyFactory));
            if (locationFactory == null) throw new ArgumentNullException(nameof(locationFactory));
            if (underlyingObjectFactory == null) throw new ArgumentNullException(nameof(underlyingObjectFactory));

            this.propertyFactory = propertyFactory;
            this.locationFactory = locationFactory;
            this.underlyingObjectFactory = underlyingObjectFactory;
        }

        public PropertyType TargetType => PropertyType.InvocationExpression;

        public ISyntaxNodeDeclaration Convert(PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            var location = propertyDeclarationSyntax.GetLocation();
            var locationLineSpan = location.GetLineSpan();
            var declaration = CreateLocation(locationLineSpan.StartLinePosition);

            var body = GetBody(propertyDeclarationSyntax);
            var bodyLocation = body.GetLocation();
            var bodyLineSpan = bodyLocation.GetLineSpan();
            var bodyStart = CreateLocation(bodyLineSpan.StartLinePosition);
            var bodyEnd = CreateLocation(bodyLineSpan.EndLinePosition);

            return CreateProperty(declaration, bodyStart, bodyEnd, body);
        }

        protected ArrowExpressionClauseSyntax GetBody(PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            return propertyDeclarationSyntax.ExpressionBody;
        }

        private ILocation CreateLocation(LinePosition linePosition)
        {
            return locationFactory.Create(linePosition.Line, linePosition.Character);
        }

        private IProperty CreateProperty(
            ILocation declaration,
            ILocation bodyStart,
            ILocation bodyEnd,
            ArrowExpressionClauseSyntax expressionBody)
        {
            var underlyingSyntaxNode = underlyingObjectFactory.Create(expressionBody);
            return propertyFactory.Create(declaration, bodyStart, bodyEnd, underlyingSyntaxNode);
        }
    }
}