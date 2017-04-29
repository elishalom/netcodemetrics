using System.Windows.Media;
using CodeMetrics.Calculators.Contracts;
using CodeMetrics.Options;
using CodeMetrics.UserControls;
using Moq;
using NUnit.Framework;

namespace CodeMetrics.Adornments.Tests
{
    [TestFixture]
    public class ComplexityViewModelTests
    {
        private const int Expected = 5;

        [Test]
        public void NewComplexity_UpdateComplexity_FiresNotifyPropertyChanged()
        {
            bool eventFired = false;
            var model = CreateViewModel();
            model.PropertyChanged += (sender, args) => eventFired = true;
            var complexityMock = CreateComplexityMock();
            model.UpdateComplexity(complexityMock.Object);
            Assert.That(eventFired, Is.True, "ViewMoled bindings in UI require property working notify property changed.");
        }

        [Test]
        public void NoComplexitySet_GetValue_Returns0()
        {
            ComplexityViewModel model = CreateViewModel();
            Assert.That(model.Value, Is.EqualTo(0), "Default complexity value should be zero.");
        }

        [Test]
        public void AfterUpdateComplexity5_GetValue_Returns5()
        {
            ComplexityViewModel model = AssignComplexity();
            Assert.That(model.Value, Is.EqualTo(Expected), "View model wraps last assigned complexity.");
        }

        [Test]
        public void AfterUpdateComplexity0_GetColor_ReturnsGreen()
        {
            ComplexityViewModel model = AssignComplexity(0);
            Assert.That(model.Color, Is.EqualTo(Colors.Green), "Zero complexity is converted to allowed color..");
        }

        [Test]
        public void MiminumToShow3Complexity5_GetVisible_ReturnsTrue()
        {
            ComplexityViewModel model = CreateViewModelByOptions(3);
            Assert.That(model.Visible, Is.True);
        }

        [Test]
        public void MiminumToShow5Complexity5_GetVisible_ReturnsTrue()
        {
            ComplexityViewModel model = CreateViewModelByOptions(5);
            Assert.That(model.Visible, Is.True);
        }

        [Test]
        public void MiminumToShow7Complexity5_GetVisible_ReturnsFalse()
        {
            ComplexityViewModel model = CreateViewModelByOptions(7);
            Assert.That(model.Visible, Is.False);
        }

        private static ComplexityViewModel CreateViewModelByOptions(int minimumToShow)
        {
            Mock<IOptions> optionsMock = new Mock<IOptions>();
            optionsMock.SetupGet(o => o.MinimumToShow).Returns(minimumToShow);
            return AssigComplexity(optionsMock);
        }

        private static ComplexityViewModel AssignComplexity(int expected = Expected)
        {
            Mock<IOptions> optionsMock = ColorConverterTest.CreateOptionsMock();
            return AssigComplexity(optionsMock, expected);
        }

        private static ComplexityViewModel AssigComplexity(Mock<IOptions> optionsMock, int expected = Expected)
        {
            Mock<ICyclomaticComplexity> mockComplexity = CreateComplexityMock(expected);
            var model = new ComplexityViewModel(optionsMock.Object);
            model.UpdateComplexity(mockComplexity.Object);
            return model;
        }

        private static ComplexityViewModel CreateViewModel()
        {
            Mock<IOptions> optionsMock = ColorConverterTest.CreateOptionsMock();
            return new ComplexityViewModel(optionsMock.Object);
        }

        private static Mock<ICyclomaticComplexity> CreateComplexityMock(int expected = Expected)
        {
            var mockComplexity = new Mock<ICyclomaticComplexity>();
            mockComplexity.SetupGet(c => c.Value)
                .Returns(() => expected);
            return mockComplexity;
        }
    }
}
