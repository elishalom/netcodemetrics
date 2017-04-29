using CodeMetrics.Parsing;
using NUnit.Framework;

namespace CodeMetrics.Calculators.Tests
{
    [TestFixture]
    internal class ToStringTests
    {
        [Test]
        public void ComplexityToString_ReturnsValue()
        {
            var complexity = new CyclomaticComplexity(5);
            string complexityText = complexity.ToString();
            Assert.AreEqual("Complexity:5", complexityText, "Complexity ToString should return formated value.");
        }
    }
}
