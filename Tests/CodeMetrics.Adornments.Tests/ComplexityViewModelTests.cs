using System.Windows.Media;
using CodeMetrics.Calculators;
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

        private static ComplexityViewModel AssignComplexity(int expected = Expected)
        {
            var mockComplexity = CreateComplexityMock(expected);

            var model = CreateViewModel();
            model.UpdateComplexity(mockComplexity.Object);
            return model;
        }

        private static ComplexityViewModel CreateViewModel()
        {
            var optionsMock = ColorConverterTest.CreateOptionsMock();
            var model = new ComplexityViewModel(optionsMock.Object);
            return model;
        }

        private static Mock<IComplexity> CreateComplexityMock(int expected = Expected)
        {
            var mockComplexity = new Mock<IComplexity>();
            mockComplexity.SetupGet(c => c.Value)
                .Returns(() => expected);
            return mockComplexity;
        }
    }
}
