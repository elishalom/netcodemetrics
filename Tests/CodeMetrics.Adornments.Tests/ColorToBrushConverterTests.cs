using System;
using System.Globalization;
using System.Windows.Media;
using NUnit.Framework;

namespace CodeMetrics.Adornments.Tests
{
    [TestFixture]
    public class ColorToBrushConverterTests
    {
        [Test]
        public void Null_Convert_ReturnTransparentBrush()
        {
            var brush = this.Convert(null);
            Assert.That(brush, Is.EqualTo(Brushes.Transparent));
        }

        [Test]
        public void Object_Convert_ReturnTransparentBrush()
        {
            var brush = this.Convert(new object());
            Assert.That(brush, Is.EqualTo(Brushes.Transparent));
        }

        [Test]
        public void GreenColor_Convert_ReturnGreenBrush()
        {
            Color expected = Colors.Green;
            var brush = this.Convert(expected);
            Assert.That(brush.Color, Is.EqualTo(expected));
        }

        [Test]
        public void TransparentBrush_ConvertBack_ThrowsNotImplementedException()
        {
            var converter = new ColorToBrushConverter();
            
            Assert.That(() => converter.ConvertBack(Brushes.Transparent, typeof(Brush), null, CultureInfo.InvariantCulture),
                Throws.TypeOf<NotImplementedException>());
        }

        private SolidColorBrush Convert(object toConvert)
        {
            var converter = new ColorToBrushConverter();
            return (SolidColorBrush)converter.Convert(toConvert, typeof(Color), null, CultureInfo.InvariantCulture);
        }
    }
}
