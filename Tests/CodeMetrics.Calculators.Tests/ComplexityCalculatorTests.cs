using NUnit.Framework;

namespace CodeMetrics.Calculators.Tests
{
    [TestFixture]
    public class ComplexityCalculatorTests
    {
        [Test]
        public void Calculate_MethodWithSinglePath_Return1()
        {
            const string method =
@"int x = 0;";

            var calculator = new ComplexityCalculator();
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

            var calculator = new ComplexityCalculator();
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

            var calculator = new ComplexityCalculator();
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