using System.Collections.Generic;
using System.Linq;
using CodeMetrics.Common;
using NUnit.Framework;

namespace CodeMetrics.Parsing.Tests
{
    /// <summary>
    /// learning tests for parsing the class Constructors
    /// </summary>
    [TestFixture]
    internal class ConstructorExtractorTests
    {
        private IMethodsVisitorFactory factory;

        [SetUp]
        public void Setup()
        {
            var windsorContainer = ContainerFactory.CreateContainer();
            factory = windsorContainer.Resolve<IMethodsVisitorFactory>();
        }

        [Test]
        public void Extract_FileWithSingleClassWithNoConstructor_ReturnNoConstructor()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
    }
}";
            var methods = ExtractConstructors(fileCode);
            
            Assert.That(methods, Is.Empty);
        }

        [Test]
        public void Extract_FileWithSingleClassWithParameterLessConstructor_ReturnsOneMethod()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public MyCalss()
        {
        }
    }
}";
            var methods = ExtractConstructors(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Extract_FileWithSingleClassWithParameterConstructor_ReturnOneMethod()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public MyCalss(string construtorParam)
        {
        }
    }
}";
            var methods = ExtractConstructors(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Extract_FileWithSingleClassWithTwoConstructors_ReturnTwoMethods()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public MyCalss(string construtorParam)
        {
        }

        public MyCalss(int construtorParam)
        {
        }
    }
}";
            var methods = ExtractConstructors(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Extract_FileWithSingleClassWithDerivedConstructor_ReturnOneMethod()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public MyCalss(string construtorParam) : base()
        {
        }
    }
}";
            var methods = ExtractConstructors(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Extract_FileWithSingleClassWithReferencedConstructor_ReturnTwoMethods()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public MyCalss(string construtorParam) : this()
        {
        }

        public MyCalss()
        {
        }
    }
}";
            var methods = ExtractConstructors(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Extract_FileWithTwoClassesWithOneConstructor_ReturnTwoMethods()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public MyCalss()
        {
        }
    }

    public class MyCalssB
    {
        public MyCalssB()
        {
        }
    }
}";
            var methods = ExtractConstructors(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Extract_FileWithNestedClassWithOneConstructor_ReturnTwoMethods()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public MyCalss()
        {
        }

        public class MyCalssB
        {
            public MyCalssB()
            {
            }
        }
    }
}";
            var methods = ExtractConstructors(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Extract_FileWithOneClassWithInLineConstructor_ReturnOneMethod()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public MyCalss() {        }
    }
}";
            var methods = ExtractConstructors(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(1));
        }

        private IEnumerable<IMethod> ExtractConstructors(string fileCode)
        {
            var methodsExtractor = new MethodsExtractor(factory);
            return methodsExtractor.Extract(fileCode);
        }
    }
}
