using System;
using CodeMetrics.Common;
using CodeMetrics.Parsing;
using ICSharpCode.NRefactory.CSharp;
using Moq;
using NUnit.Framework;

namespace CodeMetrics.Calculators.Tests
{
    [TestFixture]
    public class ComplexityCalculatorTests
    {
        private IBranchesVisitorFactory factory;

        [SetUp]
        public void Setup()
        {
            var windsorContainer = ContainerFactory.CreateContainer();
            factory = windsorContainer.Resolve<IBranchesVisitorFactory>();
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

        [Test, Ignore("Not supported yet")]
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

        private IComplexity CalculateMethodComplexity(string method)
        {
            var calculator = new ComplexityCalculator(factory);
            return calculator.Calculate(method);
        }

        [Test]
        public void Calculate_ThrownsNullReferenceException_Returns1()
        {
            // here the method it self doesnt have to be invalid, the factory is failing
            const string method = @"if(b1) { }";
            var stubFactory = SetupFailingStubFactory();

            var calculator = new ComplexityCalculator(stubFactory.Object);
            var complexity = calculator.Calculate(method);

            Assert.That(complexity.Value, Is.EqualTo(1));
        }

        private static Mock<IBranchesVisitorFactory> SetupFailingStubFactory()
        {
            var failinVisitor = new Mock<DepthFirstAstVisitor>();
            var falingCalculator = failinVisitor.As<IBranchesVisitor>();
            failinVisitor.Setup(visitor => visitor.VisitIfElseStatement(It.IsAny<IfElseStatement>()))
                         .Throws<NullReferenceException>();
            var stubFactory = new Mock<IBranchesVisitorFactory>();
            stubFactory.Setup(stub => stub.CreateBranchesVisitor())
                       .Returns(falingCalculator.Object);
            return stubFactory;
        }
    }
}
