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

        [ExpectedException(typeof(NotImplementedException))]
        [Test]
        public void BrushIsNotConvertibleBackToComplexity()
        {
            var converter = new ComplexityToColor();
            converter.ConvertBack(new SolidColorBrush(), typeof(int), null, CultureInfo.InvariantCulture);
        }


        private static Color ConvertToBrush(int complexity)
        {
            var converter = new ComplexityToColor();
            var color = (Color)converter.Convert(complexity, typeof(Color),
                null, CultureInfo.InvariantCulture);
            return color;
        }
    }
}
