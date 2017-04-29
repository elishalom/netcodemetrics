using System;
using System.Collections.Generic;
using System.Linq;
using CodeMetrics.Parsing.Contracts;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Roslyn
{
    public class MethodExtractor : IMethodExtractor
    {
        private readonly IEnumerable<IMethodConvertor> convertors;
        private readonly IMethodTypeResolver methodTypeResolver;

        public MethodExtractor(IMethodTypeResolver methodTypeResolver, IEnumerable<IMethodConvertor> convertors)
        {
            if (methodTypeResolver == null) throw new ArgumentNullException(nameof(methodTypeResolver));
            if (convertors == null) throw new ArgumentNullException(nameof(convertors));

            this.methodTypeResolver = methodTypeResolver;
            this.convertors = convertors;
        }

        public IEnumerable<ISyntaxNodeDeclaration> Extract(string fileCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(fileCode);
            var root = (CompilationUnitSyntax)syntaxTree.GetRoot();
            var methods = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Select(Convert);

            return methods;
        }

        private ISyntaxNodeDeclaration Convert(MethodDeclarationSyntax declarationSyntax)
        {
            var methodType = GetMethodType(declarationSyntax);
            var convertor = convertors.First(c => c.TargetType == methodType);
            return convertor.Convert(declarationSyntax);
        }

        private MethodType GetMethodType(MethodDeclarationSyntax declarationSyntax)
        {
            return methodTypeResolver.GetMethodType(declarationSyntax);
        }
    }
}