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
    public class ConstructorExtractor : IConstructorExtractor
    {
        private readonly ILocationFactory locationFactory;
        private readonly IUnderlyingObjectFactory underlyingObjectFactory;
        private readonly IConstructorFactory constructorFactory;

        public ConstructorExtractor(IConstructorFactory constructorFactory, ILocationFactory locationFactory, IUnderlyingObjectFactory underlyingObjectFactory)
        {
            if (constructorFactory == null) throw new ArgumentNullException(nameof(constructorFactory));
            if (locationFactory == null) throw new ArgumentNullException(nameof(locationFactory));
            if (underlyingObjectFactory == null) throw new ArgumentNullException(nameof(underlyingObjectFactory));

            this.constructorFactory = constructorFactory;
            this.locationFactory = locationFactory;
            this.underlyingObjectFactory = underlyingObjectFactory;
        }

        public IEnumerable<ISyntaxNodeDeclaration> Extract(string fileCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(fileCode);
            var root = (CompilationUnitSyntax)syntaxTree.GetRoot();
            var constructors = root.DescendantNodes()
                .OfType<ConstructorDeclarationSyntax>()
                .Select(Convert);

            return constructors;
        }

        public virtual ISyntaxNodeDeclaration Convert(ConstructorDeclarationSyntax constructorDeclarationSyntax)
        {
            var location = constructorDeclarationSyntax.GetLocation();
            var locationLineSpan = location.GetLineSpan();
            var declaration = CreateLocation(locationLineSpan.StartLinePosition);

            var body = GetBody(constructorDeclarationSyntax);
            var bodyLocation = body.GetLocation();
            var bodyLineSpan = bodyLocation.GetLineSpan();
            var bodyStart = CreateLocation(bodyLineSpan.StartLinePosition);
            var bodyEnd = CreateLocation(bodyLineSpan.EndLinePosition);

            return CreateConstructor(declaration, bodyStart, bodyEnd, body);
        }

        private BlockSyntax GetBody(ConstructorDeclarationSyntax constructorDeclarationSyntax)
        {
            return constructorDeclarationSyntax.Body;
        }

        private ILocation CreateLocation(LinePosition linePosition)
        {
            return locationFactory.Create(linePosition.Line, linePosition.Character);
        }

        private IConstructor CreateConstructor(ILocation declaration, ILocation bodyStart, ILocation bodyEnd, BlockSyntax body)
        {
            var underlyingSyntaxNode = underlyingObjectFactory.Create(body);
            return constructorFactory.Create(declaration, bodyStart, bodyEnd, underlyingSyntaxNode);
        }
    }
}