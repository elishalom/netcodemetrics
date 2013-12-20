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
    internal class ConstructorExtractorTests : ExtractorsTestBase
    {
        [Test]
        public void Extract_FileWithSingleClassWithNoConstructor_ReturnNoConstructor()
        {
            var methods = ExtractMethods(OneEmptyClass);
            
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
            var methods = ExtractMethods(fileCode);

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
            var methods = ExtractMethods(fileCode);

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
            var methods = ExtractMethods(fileCode);

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
            var methods = ExtractMethods(fileCode);

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
            var methods = ExtractMethods(fileCode);

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
            var methods = ExtractMethods(fileCode);

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
            var methods = ExtractMethods(fileCode);

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
            var methods = ExtractMethods(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(1));
        }
    }
}
