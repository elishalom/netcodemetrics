using System.Windows.Media;
using CodeMetrics.Options;
using Moq;
using NUnit.Framework;

namespace CodeMetrics.Adornments.Tests
{
    [TestFixture]
    public class ColorConverterTest
    {
        [Test]
        public void ComplexityZeroConvertsToDefaultAllowedColor()
        {
            const int complexity = 0;
            Color color = ConvertToBrush(complexity);

            Assert.That(color, Is.EqualTo(Colors.Green), "Complexity 0 has to equal to default Allowed color");
        }

        [Test]
        public void ComplexityTenConvertsToDefaultAllowedColor()
        {
            const int complexity = 10;
            Color color = ConvertToBrush(complexity);

            Assert.That(color, Is.EqualTo(Colors.Red), "Complexity 10 has to equal to default Error color");
        }

        private static Color ConvertToBrush(int complexity)
        {
            Mock<IOptions> optionsMock = CreateOptionsMock();

            var converter = new ComplexityToColor(optionsMock.Object);
            return converter.Convert(complexity);
        }

        internal static Mock<IOptions> CreateOptionsMock()
        {
            var optionsMock = new Mock<IOptions>();
            optionsMock.SetupGet(o => o.MinimumToShow).Returns(0);
            optionsMock.SetupGet(o => o.GoodColor).Returns(System.Drawing.Color.Green);
            optionsMock.SetupGet(o => o.BadColor).Returns(System.Drawing.Color.Red);
            return optionsMock;
        }
    }
}
