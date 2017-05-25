using System;
using System.Collections.Generic;
using System.Linq;
using CodeMetrics.Parsing.Contracts;
using CodeMetrics.Parsing.Contracts.Roslyn;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeMetrics.Parsing.Roslyn
{
    public class PropertyExtractor : IPropertyExtractor
    {
        private readonly IEnumerable<IPropertyConvertor> convertors;
        private readonly IPropertyTypeResolver propertyTypeResolver;

        public PropertyExtractor(IPropertyTypeResolver propertyTypeResolver, IEnumerable<IPropertyConvertor> convertors)
        {
            if (propertyTypeResolver == null) throw new ArgumentNullException(nameof(propertyTypeResolver));
            if (convertors == null) throw new ArgumentNullException(nameof(convertors));

            this.propertyTypeResolver = propertyTypeResolver;
            this.convertors = convertors;
        }

        public IEnumerable<ISyntaxNodeDeclaration> Extract(string fileCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(fileCode);
            var root = (CompilationUnitSyntax)syntaxTree.GetRoot();
            var methods = root.DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .Select(Convert);

            return methods.Where(snd => snd != null);
        }

        private ISyntaxNodeDeclaration Convert(PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            try
            {
                var propertyType = GetMethodType(propertyDeclarationSyntax);
                var convertor = convertors.First(c => c.TargetType == propertyType);
                return convertor.Convert(propertyDeclarationSyntax);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        private PropertyType GetMethodType(PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            return propertyTypeResolver.GetPropertyType(propertyDeclarationSyntax);
        }
    }
}