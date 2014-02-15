using System.Collections.Generic;
using CodeMetrics.Common;
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
    public class MyCalss
    {
    }
}";

        private IMethodsVisitorFactory factory;

        [SetUp]
        public void Setup()
        {
            var windsorContainer = ContainerFactory.CreateContainer();
            factory = windsorContainer.Resolve<IMethodsVisitorFactory>();
        }

        protected IEnumerable<IMethod> ExtractMethods(string fileCode)
        {
            var methodsExtractor = new MethodsExtractor(factory);
            return methodsExtractor.Extract(fileCode);
        }
    }
}
