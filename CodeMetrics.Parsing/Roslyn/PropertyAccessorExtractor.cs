using System;
using System.Collections.Generic;
using System.Linq;
using CodeMetrics.Parsing.Contracts;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CodeMetrics.Parsing.Roslyn
{
    public class PropertyAccessorExtractor : IPropertyAccessorExtractor
    {
        private readonly ILocationFactory locationFactory;
        private readonly IUnderlyingObjectFactory underlyingObjectFactory;
        private readonly IPropertyAccessorFactory propertyAccessorFactory;

        public PropertyAccessorExtractor(
            IPropertyAccessorFactory propertyAccessorFactory,
            ILocationFactory locationFactory,
            IUnderlyingObjectFactory underlyingObjectFactory)
        {
            if (propertyAccessorFactory == null) throw new ArgumentNullException(nameof(propertyAccessorFactory));
            if (locationFactory == null) throw new ArgumentNullException(nameof(locationFactory));
            if (underlyingObjectFactory == null) throw new ArgumentNullException(nameof(underlyingObjectFactory));

            this.propertyAccessorFactory = propertyAccessorFactory;
            this.locationFactory = locationFactory;
            this.underlyingObjectFactory = underlyingObjectFactory;
        }

        public IEnumerable<ISyntaxNodeDeclaration> Extract(string fileCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(fileCode);
            var root = (CompilationUnitSyntax)syntaxTree.GetRoot();
            var propertyDeclarations = root.DescendantNodes()
                .OfType<PropertyDeclarationSyntax>();

            var accessorList = propertyDeclarations
                .SelectMany(p => p.DescendantNodes().OfType<AccessorDeclarationSyntax>().Where(a => a.Body != null));

            var propertySyntaxDeclarations = accessorList.Select(Convert);
            return propertySyntaxDeclarations;
        }

        public virtual ISyntaxNodeDeclaration Convert(AccessorDeclarationSyntax accessorDeclarationSyntax)
        {
            var location = accessorDeclarationSyntax.GetLocation();
            var locationLineSpan = location.GetLineSpan();
            var declaration = CreateLocation(locationLineSpan.StartLinePosition);

            var body = accessorDeclarationSyntax.Body;
            var bodyLocation = body.GetLocation();
            var bodyLineSpan = bodyLocation.GetLineSpan();
            var bodyStart = CreateLocation(bodyLineSpan.StartLinePosition);
            var bodyEnd = CreateLocation(bodyLineSpan.EndLinePosition);

            return CreatePropertyAccessor(declaration, bodyStart, bodyEnd, body);
        }

        private ILocation CreateLocation(LinePosition linePosition)
        {
            return locationFactory.Create(linePosition.Line, linePosition.Character);
        }

        private IPropertyAccessor CreatePropertyAccessor(ILocation declaration, ILocation bodyStart, ILocation bodyEnd, BlockSyntax body)
        {
            var underlyingSyntaxNode = underlyingObjectFactory.Create(body);
            return propertyAccessorFactory.Create(declaration, bodyStart, bodyEnd, underlyingSyntaxNode);
        }
    }
}