using NUnit.Framework;

namespace CodeMetrics.Parsing.Tests
{
    [TestFixture]
    internal class ToStringTests
    {
        [Test]
        public void ValidLocationToString_ReturnsLineAndColumn()
        {
            var location = new Location(1, 2);
            var locationText = location.ToString();
            Assert.AreEqual("Location:1,2", locationText, "Location ToString method should return line and column values");
        }

        [Test]
        public void ValidMethodToString_ReturnsAllConcatenatedLocations()
        {
            var declaration = new Location(1, 2);
            var start = new Location(2, 3);
            var end = new Location(4, 5);
            var method = new Method(declaration, start, end, null);
            var methodText = method.ToString();
            Assert.AreEqual("Method:[1,2-2,3-4,5]", methodText, "Method ToString method should return line and column values");
        }
    }
}
