using System;
using System.Collections.Generic;
using System.Linq;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Roslyn
{
    public class MethodTypeResolver : IMethodTypeResolver
    {
        private readonly IEnumerable<IMethodTypeResolvingRule> resolvingRules;

        public MethodTypeResolver(IEnumerable<IMethodTypeResolvingRule> resolvingRules)
        {
            if (resolvingRules == null) throw new ArgumentNullException(nameof(resolvingRules));

            this.resolvingRules = resolvingRules;
        }

        public MethodType GetMethodType(MethodDeclarationSyntax declarationSyntax)
        {
            var rule = resolvingRules.FirstOrDefault(r => r.IsSuitable(declarationSyntax));
            if (rule != null)
            {
                return rule.MethodType;
            }

            throw new InvalidOperationException("Not supported MethodDeclarationSyntax!");
        }
    }
}