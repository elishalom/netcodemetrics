using System;
using System.Collections.Generic;
using System.Linq;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Roslyn
{
    public class PropertyTypeResolver : IPropertyTypeResolver
    {
        private readonly IEnumerable<IPropertyTypeResolvingRule> resolvingRules;

        public PropertyTypeResolver(IEnumerable<IPropertyTypeResolvingRule> resolvingRules)
        {
            if (resolvingRules == null) throw new ArgumentNullException(nameof(resolvingRules));

            this.resolvingRules = resolvingRules;
        }

        public PropertyType GetPropertyType(PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            var rule = resolvingRules.FirstOrDefault(r => r.IsSuitable(propertyDeclarationSyntax));
            if (rule != null)
            {
                return rule.PropertyType;
            }

            throw new InvalidOperationException("Not supported PropertyDeclarationSyntax!");
        }
    }
}