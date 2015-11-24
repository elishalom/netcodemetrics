using System;
using System.Globalization;
using System.Windows.Media;
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
            var converter = new ComplexityToColor();
            return converter.Convert(complexity);
        }
    }
}
