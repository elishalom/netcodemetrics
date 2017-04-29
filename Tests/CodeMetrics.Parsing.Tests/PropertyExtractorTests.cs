using System.Linq;
using CodeMetrics.Common;
using NUnit.Framework;

namespace CodeMetrics.Parsing.Tests
{
    [TestFixture]
    public class PropertyExtractorTests : ExtractorsTestBase
    {
        private const string AutomaticPropertyOnTwoLines = @"using System;
namespace MyNamespace
{
    public class MyClass
    {
        public string MyProperty
        {
             get;
             set;
        }
    }
}";

        private const string FieldPropertyOnTwoLines = @"using System;
namespace MyNamespace
{
    public class MyClass
    {
        public string MyProperty
        {
             get { return string.Empty; }
             set { }
        }
    }
}";

        [Test]
        public void Extract_FileWithSingleClassWithAutomaticProperty_IgnoresAccessors()
        {
            var methods = ExtractSyntaxNodes(AutomaticPropertyOnTwoLines);

            Assert.That(methods, Is.Empty);
        }

        [Test]
        public void Extract_FileWithSingleClassWithFieldProperty_CorrectGetterLine()
        {
            var methods = ExtractSyntaxNodes(FieldPropertyOnTwoLines);
            var getterStart = methods.First().BodyStart.Line;

            Assert.That(getterStart, Is.EqualTo(7));
        }

        [Test]
        public void Extract_FileWithSingleClassWithFieldProperty_CorrectSetterLine()
        {
            var methods = ExtractSyntaxNodes(FieldPropertyOnTwoLines);
            var setterStart = methods.ToList()[1].BodyStart.Line;

            Assert.That(setterStart, Is.EqualTo(8));
        }

        [Test]
        public void Extract_FileWithSingleClassWithNoProperty_ReturnNoMethod()
        {
            var methods = ExtractSyntaxNodes(OneEmptyClass);

            Assert.That(methods, Is.Empty);
        }

        [Test]
        public void Extract_FileWithSingleClassWithAutomaticProperty_IgnoresProperty()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyClass
    {
        public string MyProperty { get; set; }
    }
}";

            var methods = ExtractSyntaxNodes(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Extract_FileWithSingleClassWithGetterOnlyProperty_ReturnOneMethod()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyClass
    {
        public string MyProperty
        {
            get { return string.Empty; }
        }
    }
}";

            var methods = ExtractSyntaxNodes(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Extract_FileWithSingleClassWithSetterOnlyProperty_ReturnOneMethod()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyClass
    {
        public string MyProperty { set { } }
    }
}";

            var methods = ExtractSyntaxNodes(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Extract_FileWithSingleInterfaceWithGetterOnlyProperty_ReturnNoneMethod()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public interface IClass
    {
        string MyProperty { get; }
    }
}";

            var methods = ExtractSyntaxNodes(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Extract_FileWithExpressionBodyPropertyUsingRoslyn_MethodStartColumnBodyIsCorrect()
        {
            AssertContainerType(ContainerSettings.ROSLYN_INSTALLER_TYPE_NAME);

            const string fileCode = @"using System;

namespace MyNamespace
{
    public class MyClass
    {
        public DateTime CurrentDate
            => DateTime.Now;
    }
}";

            var methods = ExtractSyntaxNodes(fileCode);
            var method = methods.First();

            Assert.That(method.BodyStart.Column, Is.EqualTo(12));
        }

        [Test]
        public void Extract_FileWithExpressionBodyPropertyUsingNRefactory_MethodStartColumnBodyIsCorrect()
        {
            AssertContainerType(ContainerSettings.NREFACTORY_INSTALLER_TYPE_NAME);

            const string fileCode = @"using System;

namespace MyNamespace
{
    public class MyClass
    {
        public DateTime CurrentDate
            => DateTime.Now;
    }
}";

            var methods = ExtractSyntaxNodes(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(0));
        }
    }
}