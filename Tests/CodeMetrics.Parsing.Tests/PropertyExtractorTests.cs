using System.Linq;
using NUnit.Framework;

namespace CodeMetrics.Parsing.Tests
{
    [TestFixture]
    public class PropertyExtractorTests : ExtractorsTestBase
    {
        private const string AutomaticPropertyOnTwoLines = @"using System;
namespace MyNamespace
{
    public class MyCalss
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
    public class MyCalss
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
            var methods = ExtractMethods(AutomaticPropertyOnTwoLines);

            Assert.That(methods, Is.Empty);
        }

        [Test]
        public void Extract_FileWithSingleClassWithFieldProperty_CorrectGetterLine()
        {
            var methods = ExtractMethods(FieldPropertyOnTwoLines);
            int getterStart = methods.First().BodyStart.Line;

            Assert.That(getterStart, Is.EqualTo(7));
        }

        [Test]
        public void Extract_FileWithSingleClassWithFieldProperty_CorrectSetterLine()
        {
            var methods = ExtractMethods(FieldPropertyOnTwoLines);
            int setterStart = methods.ToList()[1].BodyStart.Line;

            Assert.That(setterStart, Is.EqualTo(8));
        }

        [Test]
        public void Extract_FileWithSingleClassWithNoProperty_ReturnNoMethod()
        {
            var methods = ExtractMethods(OneEmptyClass);

            Assert.That(methods, Is.Empty);
        }

        [Test]
        public void Extract_FileWithSingleClassWithAutomaticProperty_IgnoresProperty()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public string MyProperty { get; set; }
    }
}";

            var methods = ExtractMethods(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Extract_FileWithSingleClassWithGetterOnlyProperty_ReturnOneMethod()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public string MyProperty
        { 
            get { return string.Empty; }
        }
    }
}";

            var methods = ExtractMethods(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Extract_FileWithSingleClassWithSetterOnlyProperty_ReturnOneMethod()
        {
            const string fileCode = @"using System;
namespace MyNamespace
{
    public class MyCalss
    {
        public string MyProperty { set { } }
    }
}";

            var methods = ExtractMethods(fileCode);

            Assert.That(methods.Count(), Is.EqualTo(1));
        }
    }
}
