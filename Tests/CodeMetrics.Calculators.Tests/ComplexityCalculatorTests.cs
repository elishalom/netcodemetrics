using System;
using CodeMetrics.Common;
using NUnit.Framework;

namespace CodeMetrics.Calculators.Tests
{
    [TestFixture]
    public class ComplexityCalculatorTests
    {
        private IMethodsVisitorFactory factory;

        [SetUp]
        public void Setup()
        {
            var windsorContainer = ContainerFactory.CreateContainer();
            factory = windsorContainer.Resolve<IMethodsVisitorFactory>();
        }

        [Test]
        public void Calculate_MethodWithSinglePath_Return1()
        {
            const string method =
@"int x = 0;";

            var calculator = new ComplexityCalculator(factory);
            var complexity = calculator.Calculate(method);

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

            var calculator = new ComplexityCalculator(factory);
            var complexity = calculator.Calculate(method);

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

            var calculator = new ComplexityCalculator(factory);
            var complexity = calculator.Calculate(method);

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

            var calculator = new ComplexityCalculator(factory);
            var complexity = calculator.Calculate(method);

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

            var calculator = new ComplexityCalculator(factory);
            var complexity = calculator.Calculate(method);

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

            var calculator = new ComplexityCalculator(factory);
            var complexity = calculator.Calculate(method);

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

            var calculator = new ComplexityCalculator(factory);
            var complexity = calculator.Calculate(method);

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

            var calculator = new ComplexityCalculator(factory);
            var complexity = calculator.Calculate(method);

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

            var calculator = new ComplexityCalculator(factory);
            var complexity = calculator.Calculate(method);

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

            var calculator = new ComplexityCalculator(factory);
            var complexity = calculator.Calculate(method);

            Assert.That(complexity.Value, Is.EqualTo(3));
        }

        public void Method(bool b)
        {
            while (b)
            {
                int x = 1;
            }
        }
    }
}
