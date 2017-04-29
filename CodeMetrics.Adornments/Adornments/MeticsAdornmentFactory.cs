using System.ComponentModel.Composition;
using CodeMetrics.Calculators.Contracts;
using CodeMetrics.Common;
using CodeMetrics.Parsing.Contracts;
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
            var exceptionHandler = new ExceptionHandler();
            var container = ContainerFactory.CreateContainer(exceptionHandler);

            var methodsExtractor = container.Resolve<IMethodsExtractor>();
            var complexityCalculator = container.Resolve<ICyclomaticComplexityCalculator>();
            var view = new MetricsAdornment(textView, methodsExtractor, complexityCalculator);
        }
    }
}