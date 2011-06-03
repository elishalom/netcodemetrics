using System.ComponentModel.Composition;
using Castle.Windsor;
using CodeMetrics.Calculators;
using CodeMetrics.Common;
using CodeMetrics.Parsing;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace CodeMetrics.Adornments
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("csharp")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class MeticsAdornmentFactory : IWpfTextViewCreationListener
    {
        public const string ADORNMENT_NAME = "MetricsAdornment";

        [Export(typeof(AdornmentLayerDefinition))]
        [Name(ADORNMENT_NAME)]
        [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
        [TextViewRole(PredefinedTextViewRoles.Document)]
        public AdornmentLayerDefinition EditorAdornmentLayer;

        public void TextViewCreated(IWpfTextView textView)
        {
            WindsorContainer container = ContainerFactory.CreateContainer();

            var methodsExtractor = container.Resolve<IMethodsExtractor>();
            var complexityCalculator = container.Resolve<IComplexityCalculator>();
            new MetricsAdornment(textView, methodsExtractor, complexityCalculator);
        }
    }
}