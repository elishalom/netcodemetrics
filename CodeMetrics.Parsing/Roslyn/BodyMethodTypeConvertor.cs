using CodeMetrics.Parsing.Contracts;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Roslyn
{
    public class BodyMethodTypeConvertor : MethodTypeConvertorBase
    {
        public BodyMethodTypeConvertor(IMethodFactory methodFactory, ILocationFactory locationFactory, IUnderlyingObjectFactory underlyingObjectFactory)
            : base(methodFactory, locationFactory, underlyingObjectFactory)
        {
        }

        public override MethodType TargetType => MethodType.Body;

        protected override CSharpSyntaxNode GetMethodBody(MethodDeclarationSyntax methodDeclarationSyntax)
        {
            return methodDeclarationSyntax.Body;
        }
    }
}