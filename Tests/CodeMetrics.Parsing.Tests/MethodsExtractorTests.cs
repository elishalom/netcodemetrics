using System.Linq;
using CodeMetrics.Common;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace CodeMetrics.Parsing.Tests
{
    [TestFixture]
    public class MethodsExtractorTests
    {
        private IMethodsVisitorFactory factory;

        [SetUp]
        public void Setup()
        {
            var windsorContainer = ContainerFactory.CreateContainer();
            factory = windsorContainer.Resolve<IMethodsVisitorFactory>();
        }

        [Test]
        public void Extract_FileWithSingleClassWithNoMethods_ReturnNoMethods()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
    }
}";
            var methodsExtractor = new MethodsExtractor(factory);
            var methods = methodsExtractor.Extract(fileCode);

            Assert.That(methods, Is.Empty);
        }

        [Test]
        public void Extract_FileWithSingleClassWithSingleMethod_ReturnOneMethod()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public void MyMethod { }
    }
}";
            var methodsExtractor = new MethodsExtractor(factory);
            var methods = methodsExtractor.Extract(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Extract_FileWithSingleClassWithTwoMethods_ReturnTwoMethods()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public void MyMethod1 { }
        public void MyMethod2 { }
    }
}";
            var methodsExtractor = new MethodsExtractor(factory);
            var methods = methodsExtractor.Extract(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Extract_FilePrefixedWithTwoUsingsWithSingleClassWithSingleMethod_ReturnOneMethod()
        {
            const string fileCode = @"using System;
using System.Reflection;
namespace MyNamespace
{
    public class MyCalss
    {
        public void MyMethod { }
    }
}";
            var methodsExtractor = new MethodsExtractor(factory);
            var methods = methodsExtractor.Extract(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Extract_FileWithTwoClassesWithSingleMethodEach_ReturnTwoMethod()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss1
    {
        public void MyMethod1 { }
    }
    public class MyCalss2
    {
        public void MyMethod2 { }
    }
}";
            var methodsExtractor = new MethodsExtractor(factory);
            var methods = methodsExtractor.Extract(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Extract_FileWithNestedClassWithSingleMethod_ReturnOneMethod()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public class MyNestedCalss
        {
            public void MyMethod { }
        }
    }
}";
            var methodsExtractor = new MethodsExtractor(factory);
            var methods = methodsExtractor.Extract(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Extract_FileWithSingleClassWithSingleMethod_MethodStartLineIsCorrect()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public void MyMethod { }
    }
}";
            var methodsExtractor = new MethodsExtractor(factory);
            var methods = methodsExtractor.Extract(fileCode);

            Assert.That(methods.First().Decleration.Line, Is.EqualTo(5));
        }

        [Test]
        public void Extract_FileWithSingleClassWithSingleMethod_MethodStartColumnIsCorrect()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public void MyMethod { }
    }
}";
            var methodsExtractor = new MethodsExtractor(factory);
            var methods = methodsExtractor.Extract(fileCode);

            Assert.That(methods.First().Decleration.Column, Is.EqualTo(8));
        }

        [Test]
        public void Extract_FileWithSingleClassWithSingleMethod_MethodEndLineIsCorrect()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public void MyMethod { }
    }
}";
            var methodsExtractor = new MethodsExtractor(factory);
            var methods = methodsExtractor.Extract(fileCode);

            Assert.That(methods.First().BodyEnd.Line, Is.EqualTo(5));
        }
        [Test]
        public void Extract_FileWithSingleClassWithSingleMethod_MethodEndColumnIsCorrect()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public void MyMethod { }
    }
}";
            var methodsExtractor = new MethodsExtractor(factory);
            var methods = methodsExtractor.Extract(fileCode);

            Assert.That(methods.First().BodyEnd.Column, Is.EqualTo(32));
        }

        [Test]
        public void Extract_FileWithSingleClassWithSingleMethod_MethodStartLineBodyIsCorrect()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public void MyMethod
        {
        }
    }
}";
            var methodsExtractor = new MethodsExtractor(factory);
            var methods = methodsExtractor.Extract(fileCode);

            Assert.That(methods.First().BodyStart.Line, Is.EqualTo(6));
        }

        [Test]
        public void Extract_FileWithSingleClassWithSingleMethod_MethodStartColumnBodyIsCorrect()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public void MyMethod
        {
        }
    }
}";
            var methodsExtractor = new MethodsExtractor(factory);
            var methods = methodsExtractor.Extract(fileCode);

            Assert.That(methods.First().BodyStart.Column, Is.EqualTo(8));
        }


    }
}