using System;
using CodeMetrics.Parsing.Contracts;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CodeMetrics.Parsing.Roslyn
{
    public class DeclarationMethodTypeConvertor : IMethodConvertor
    {
        private readonly IMethodDeclarationFactory methodDeclarationFactory;
        private readonly ILocationFactory locationFactory;
        private readonly IUnderlyingObjectFactory underlyingObjectFactory;

        public DeclarationMethodTypeConvertor(IMethodDeclarationFactory methodDeclarationFactory, ILocationFactory locationFactory, IUnderlyingObjectFactory underlyingObjectFactory)
        {
            if (methodDeclarationFactory == null) throw new ArgumentNullException(nameof(methodDeclarationFactory));
            if (locationFactory == null) throw new ArgumentNullException(nameof(locationFactory));
            if (underlyingObjectFactory == null) throw new ArgumentNullException(nameof(underlyingObjectFactory));

            this.methodDeclarationFactory = methodDeclarationFactory;
            this.locationFactory = locationFactory;
            this.underlyingObjectFactory = underlyingObjectFactory;
        }

        public MethodType TargetType => MethodType.Declaration;

        public ISyntaxNodeDeclaration Convert(MethodDeclarationSyntax methodDeclarationSyntax)
        {
            var location = methodDeclarationSyntax.GetLocation();
            var locationLineSpan = location.GetLineSpan();
            var declaration = CreateLocation(locationLineSpan.StartLinePosition);

            return CreateMethod(declaration, methodDeclarationSyntax);
        }

        private ILocation CreateLocation(LinePosition linePosition)
        {
            return locationFactory.Create(linePosition.Line, linePosition.Character);
        }

        private IMethodDeclaration CreateMethod(ILocation declaration, MethodDeclarationSyntax methodDeclarationSyntax)
        {
            var underlyingSyntaxNode = underlyingObjectFactory.Create(methodDeclarationSyntax);
            return methodDeclarationFactory.Create(declaration, underlyingSyntaxNode);
        }
    }
}