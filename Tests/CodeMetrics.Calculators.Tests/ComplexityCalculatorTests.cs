using System;
using System.Linq;
using CodeMetrics.Calculators.Contracts;
using CodeMetrics.Common;
using CodeMetrics.Parsing.Contracts;
using NUnit.Framework;

namespace CodeMetrics.Calculators.Tests
{
    [TestFixture]
    public class ComplexityCalculatorTests
    {
        private ICyclomaticComplexityCalculator calculator;
        private IMethodsExtractor extractor;

        [SetUp]
        public void Setup()
        {
            var exceptionHandler = new TestExceptionHandler();
            var windsorContainer = ContainerFactory.CreateContainer(exceptionHandler);
            _ContainerType = ContainerSettings.ContainerType;
            var calculatorFactory = windsorContainer.Resolve<ICyclomaticComplexityCalculatorFactory>();
            calculator = calculatorFactory.Create();
            var extractorFactory = windsorContainer.Resolve<IMethodsExtractorFactory>();
            extractor = extractorFactory.Create();
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

        [Test]
        public void Calculate_MethodWithSinglePath_Return1()
        {
            const string method =
                @"int x = 0;";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(1));
        }

        [Test]
        public void Calculate_MethodWithTryCatch_Return2()
        {
            const string method =
                @"try { } catch (Exception ex) { }";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(2));
        }

        [Test]
        public void Calculate_MethodWithTryAndMultipleCatch_Return3()
        {
            const string method =
                @"try { } catch (InvalidOperationException ioe) { } catch (Exception ex) { }";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_MethodWithSingleIfWithoutElse_Return2()
        {
            const string method =
                @"if(b)
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(2));
        }

        [Test]
        public void Calculate_MethodWithSingleIfWithElse_Return3()
        {
            const string method =
                @"if(b)
{
    int x = 1;
}
else
{
    int y = 2;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_MethodWithSingleWithAndOperator_Return3()
        {
            const string method =
                @"if(b1 && b2)
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_MethodWithSingleWithOrOperator_Return3()
        {
            const string method =
                @"if(b1 || b2)
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_MethodWithTwoAndOperators_Return4()
        {
            const string method =
                @"if(b1 && b2 && b3)
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(4));
        }

        [Test]
        public void Calculate_MethodWithAndAndOrOperators_Return4()
        {
            const string method =
                @"if(b1 && b2 || b3)
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(4));
        }

        [Test]
        public void Calculate_MethodWithAndAndOrOperatorsWithBrackets_Return4()
        {
            const string method =
                @"if(b1 && (b2 || b3))
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(4));
        }

        [Test]
        public void Calculate_MethodWithForLoop_Return2()
        {
            const string method =
                @"for(int i = 0; i < 10; i++)
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(2));
        }

        [Test]
        public void Calculate_MethodWithForLoopIncludingAndOperator_Return3()
        {
            const string method =
                @"for(int i = 0; i < 10 && i > 1; i++)
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_BooleanAssignmentOfAndOperator_Return1()
        {
            const string method =
                @"bool b = b1 && b2;";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(1));
        }

        [Test]
        public void Calculate_BooleanAssignmentOfAndOperatorUsedInIfStatement_Return3()
        {
            const string method =
                @"bool b = b1 && b2;
if(b)
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_BooleanAssignmentOfOrOperatorUsedInIfStatement_Return3()
        {
            const string method =
                @"bool b = b1 || b2;
if(b)
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_BooleanAssignmentOfAndOperatorUsedInIfStatementWithNegationOperator_Return3()
        {
            const string method =
                @"bool b = b1 && b2;
if(!b)
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_ForEachLoop_Return2()
        {
            const string method =
                @"foreach(var item in items)
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(2));
        }

        [Test]
        public void Calculate_WhileLoop_Return2()
        {
            const string method =
                @"while(b)
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(2));
        }

        [Test]
        public void Calculate_BooleanAssignmentOfOrOperatorUsedInWhileStatement_Return3()
        {
            const string method =
                @"bool b = b1 || b2;
while(b)
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_BooleanAssignmentOfAndOperatorUsedInWhileStatement_Return3()
        {
            const string method =
                @"bool b = b1 && b2;
while(b)
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_WhileLoopWithAndOperator_Return3()
        {
            const string method =
                @"while(b1 && b2)
{
    int x = 1;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_DoWhileLoop_Return2()
        {
            const string method =
                @"do
{
    int x = 1;
} while(b);";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(2));
        }

        [Test]
        public void Calculate_DoWhileLoopWithAndOperator_Return3()
        {
            const string method =
                @"do
{
    int x = 1;
} while(b1 && b2);";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_BooleanAssignmentOfAndOperatorUsedInDoWhileStatement_Return3()
        {
            const string method =
                @"bool b = b1 && b2;
do
{
    int x = 1;
}while(b)";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_NestedIf_Return3()
        {
            const string method =
@"if(b1)
{
    if(b2)
    {
        int x = 1;
    }
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_SwitchWithTreeCasesWithoutDefault_Return4()
        {
            const string method =
@"switch (sf)
{
    case 0: break;
    case 1: break;
    case 2: break;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(4));
        }

        [Test]
        public void Calculate_SwitchWithTreeCasesWithDefault_Return4()
        {
            const string method =
@"switch (sf)
{
    case 0: break;
    case 1: break;
    case 2: break;
    default: break;
}";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(4));
        }

        [Test]
        public void Calculate_TrinaryOperator_Return3()
        {
            const string method =
@"int x = 1 > 0 ? 1 : 0";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_Coalescing_Operator_Return2()
        {
            const string method = @"
object a = null;
object b = a ?? string.Empty";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(2));
        }

        [Test]
        public void Calculate_SingleObjectInitializer_ShouldReturn1()
        {
            const string method = @"
var p1 = new Person
{
    Name = string.Empty
};";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(1));
        }

        [Test]
        public void Calculate_MultipleObjectInitializer_ShouldReturn1()
        {
            const string method = @"
var p1 = new Person
{
    Name = string.Empty
};
var p2 = new Person
{
    Name = string.Empty
};";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(1));
        }

        [Test]
        public void Calculate_ConditionalAccessExpression_ShouldReturn2()
        {
            AssertContainerType(ContainerSettings.ROSLYN_INSTALLER_TYPE_NAME);

            const string method = @"
var numbers = new int[] { 10, 20 };
return numbers?.ToList();
";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(2));
        }

        [Test]
        public void Calculate_ExpressionWithOrOperator_ShouldReturn3()
        {
            const string method = @"
int type = 1;
int? kind = null;
return kind == null || type != 1;
";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        [Test]
        public void Calculate_SingleObjectInitializerWithBinaryExpression_ShouldReturn2()
        {
            const string method = @"
string name = null;
var p1 = new Person
{
    Name = name ?? string.Empty
};";

            var complexity = CalculateMethodComplexity(method);

            Assert.That(complexity.Value, Is.EqualTo(2));
        }

        [Test]
        public void Calculate_SingleInterfaceWithSingleMethod_ShouldReturnDefaultValue()
        {
            const string fileCode = @"using System;

namespace SomeNamespace
{
    public interface ISomeInterface
    {
        void SomeMethod(object input);
    }
}";

            var syntaxNodes = extractor.Extract(fileCode);
            var syntaxNode = syntaxNodes.OfType<ISyntaxNode>().FirstOrDefault();

            var complexity = calculator.Calculate(syntaxNode);
            Assert.That(complexity.Value, Is.EqualTo(1));
        }

        [Test]
        public void Calculate_SingleAbstractClassWithSingleAbstractMethod_ShouldReturnDefaultValue()
        {
            const string fileCode = @"using System;

namespace SomeNamespace
{
    public abstract class SomeAbstractClass
    {
        public abstract void SomeMethod(object input);
    }
}";

            var syntaxNodes = extractor.Extract(fileCode);
            var syntaxNode = syntaxNodes.OfType<ISyntaxNode>().FirstOrDefault();

            var complexity = calculator.Calculate(syntaxNode);
            Assert.That(complexity.Value, Is.EqualTo(1));
        }

        private ICyclomaticComplexity CalculateMethodComplexity(string method)
        {
            var fileCode = @"using System;
using System.Collections.Generic;
using System.Linq;

namespace MyNamespace
{
    private class MyPerson
    {
        public string Name { get; set; }
    }

    public class MyClass
    {
        public void MyMethod()
        {
            " + method + @"
        }
    }
}";
            var syntaxNodes = extractor.Extract(fileCode);
            var syntaxNode = syntaxNodes.OfType<ISyntaxNode>().FirstOrDefault();

            return calculator.Calculate(syntaxNode);
        }
    }

    public class TestExceptionHandler : IExceptionHandler
    {
        public void HandleException(Exception exception)
        {
        }
    }
}