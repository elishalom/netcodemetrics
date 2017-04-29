using System;
using System.Collections.Generic;
using System.Linq;
using CodeMetrics.Parsing.Contracts;
using CodeMetrics.Parsing.Contracts.Roslyn;

namespace CodeMetrics.Parsing.Roslyn
{
    /// <summary>
    /// Extract syntax nodes by following extractors
    /// - Constructor
    /// - Property
    /// - PropertyAccessor
    /// - Method
    /// </summary>
    /// <seealso cref="IMethodsExtractor" />
    public class CSharpMethodsExtractor : IMethodsExtractor
    {
        private readonly IEnumerable<ISyntaxNodeExtractor> extractors;
        private readonly IExceptionHandler exceptionHandler;

        public CSharpMethodsExtractor(IEnumerable<ISyntaxNodeExtractor> extractors, IExceptionHandler exceptionHandler)
        {
            if (extractors == null) throw new ArgumentNullException(nameof(extractors));
            if (exceptionHandler == null) throw new ArgumentNullException(nameof(exceptionHandler));

            this.extractors = extractors;
            this.exceptionHandler = exceptionHandler;
        }

        public IEnumerable<ISyntaxNodeDeclaration> Extract(string fileCode)
        {
            return extractors.SelectMany(e => Extract(e, fileCode));
        }

        private IEnumerable<ISyntaxNodeDeclaration> Extract(ISyntaxNodeExtractor extractor, string fileCode)
        {
            try
            {
                return extractor.Extract(fileCode);
            }
            catch (Exception ex)
            {
                exceptionHandler.HandleException(ex);
            }
            return Enumerable.Empty<ISyntaxNodeDeclaration>();
        }
    }
}