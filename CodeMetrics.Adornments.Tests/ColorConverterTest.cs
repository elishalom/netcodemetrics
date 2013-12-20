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
            SolidColorBrush brush = ConvertToBrush(complexity);

            Assert.That(brush.Color, Is.EqualTo(Colors.Green), "Complexity 0 has to equal to default Allowed color");
        }

        [Test]
        public void ComplexityTenConvertsToDefaultAllowedColor()
        {
            const int complexity = 10;
            SolidColorBrush brush = ConvertToBrush(complexity);

            Assert.That(brush.Color, Is.EqualTo(Colors.Red), "Complexity 10 has to equal to default Error color");
        }

        [ExpectedException(typeof(NotImplementedException))]
        [Test]
        public void BrushIsNotConvertibleBackToComplexity()
        {
            var converter = new ComplexityToColor();
            converter.ConvertBack(new SolidColorBrush(), typeof(int), null, CultureInfo.InvariantCulture);
        }


        private static SolidColorBrush ConvertToBrush(int complexity)
        {
            var converter = new ComplexityToColor();
            var brush = (SolidColorBrush)converter.Convert(complexity, typeof(SolidColorBrush),
                null, CultureInfo.InvariantCulture);
            return brush;
        }
    }
}
