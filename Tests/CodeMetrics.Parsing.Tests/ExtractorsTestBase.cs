using System;
using System.Collections.Generic;
using System.Linq;
using CodeMetrics.Common;
using CodeMetrics.Parsing.Contracts;
using NUnit.Framework;

namespace CodeMetrics.Parsing.Tests
{
    /// <summary>
    /// Fresh Test fixture for all kind of extractors
    /// </summary>
    [TestFixture]
    public class ExtractorsTestBase
    {
        protected const string OneEmptyClass = @"using System;
namespace MyNamespace
{
    public class MyClass
    {
    }
}";

        private IMethodsExtractor extractor;

        [SetUp]
        public void Setup()
        {
            var exceptionHandler = new TestExceptionHandler();
            var windsorContainer = ContainerFactory.CreateContainer(exceptionHandler);
            _ContainerType = ContainerSettings.ContainerType;
            var factory = windsorContainer.Resolve<IMethodsExtractorFactory>();
            extractor = factory.Create();
        }

        protected IEnumerable<ISyntaxNode> ExtractSyntaxNodes(string fileCode)
        {
            return extractor.Extract(fileCode).OfType<ISyntaxNode>();
        }

        private string _ContainerType;
        protected string ContainerType => _ContainerType;

        protected void AssertContainerType(string relevantContainerType)
        {
            if (ContainerType == relevantContainerType)
            {
                return;
            }

            Assert.Inconclusive();
        }
    }

    public class TestExceptionHandler : IExceptionHandler
    {
        public void HandleException(Exception exception)
        {
        }
    }
}