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
            var model = new ComplexityViewModel();
            model.PropertyChanged += (sender, args) => eventFired = true;
            var complexityMock = CreateComplexityMock();
            model.UpdateComplexity(complexityMock.Object);
            Assert.That(eventFired, Is.True, "ViewMoled bindings in UI require property working notify property changed.");
        }

        [Test]
        public void NoComplexitySet_GetValue_Returns0()
        {
            ComplexityViewModel model = new ComplexityViewModel();
            Assert.That(model.Value, Is.EqualTo(0), "Default complexity value should be zero.");
        }

        [Test]
        public void AfterUpdateComplexity5_GetValue_Returns5()
        {
            ComplexityViewModel model = AssignComplexity();
            Assert.That(model.Value, Is.EqualTo(Expected), "View model wraps last assigned complexity.");
        }

        private static ComplexityViewModel AssignComplexity()
        {
            var mockComplexity = CreateComplexityMock();

            var model = new ComplexityViewModel();
            model.UpdateComplexity(mockComplexity.Object);
            return model;
        }

        private static Mock<IComplexity> CreateComplexityMock()
        {
            var mockComplexity = new Mock<IComplexity>();
            mockComplexity.SetupGet(c => c.Value)
                .Returns(() => Expected);
            return mockComplexity;
        }
    }
}
