using System;
using CodeMetrics.Parsing.Contracts;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CodeMetrics.Parsing.Roslyn
{
    public abstract class MethodTypeConvertorBase : IMethodConvertor
    {
        private readonly ILocationFactory locationFactory;
        private readonly IUnderlyingObjectFactory underlyingObjectFactory;
        private readonly IMethodFactory methodFactory;

        protected MethodTypeConvertorBase(IMethodFactory methodFactory, ILocationFactory locationFactory, IUnderlyingObjectFactory underlyingObjectFactory)
        {
            if (methodFactory == null) throw new ArgumentNullException(nameof(methodFactory));
            if (locationFactory == null) throw new ArgumentNullException(nameof(locationFactory));
            if (underlyingObjectFactory == null) throw new ArgumentNullException(nameof(underlyingObjectFactory));

            this.methodFactory = methodFactory;
            this.locationFactory = locationFactory;
            this.underlyingObjectFactory = underlyingObjectFactory;
        }

        public abstract MethodType TargetType { get; }

        public virtual ISyntaxNodeDeclaration Convert(MethodDeclarationSyntax methodDeclarationSyntax)
        {
            var location = methodDeclarationSyntax.GetLocation();
            var locationLineSpan = location.GetLineSpan();
            var declaration = CreateLocation(locationLineSpan.StartLinePosition);

            var methodBody = GetMethodBody(methodDeclarationSyntax);
            var methodBodyLocation = methodBody.GetLocation();
            var methodBodyLineSpan = methodBodyLocation.GetLineSpan();
            var methodBodyStart = CreateLocation(methodBodyLineSpan.StartLinePosition);
            var methodBodyEnd = CreateLocation(methodBodyLineSpan.EndLinePosition);

            return CreateMethod(declaration, methodBodyStart, methodBodyEnd, methodBody);
        }

        protected abstract CSharpSyntaxNode GetMethodBody(MethodDeclarationSyntax methodDeclarationSyntax);

        private ILocation CreateLocation(LinePosition linePosition)
        {
            return locationFactory.Create(linePosition.Line, linePosition.Character);
        }

        private IMethod CreateMethod(ILocation declaration, ILocation bodyStart, ILocation bodyEnd, CSharpSyntaxNode syntaxNode)
        {
            var underlyingSyntaxNode = underlyingObjectFactory.Create(syntaxNode);
            return methodFactory.Create(declaration, bodyStart, bodyEnd, underlyingSyntaxNode);
        }
    }
}