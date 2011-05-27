using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Castle.Windsor;
using CodeMetrics.Calculators;
using CodeMetrics.Common;
using CodeMetrics.Parsing;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace TextAdornment1
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("csharp")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class TextAdornment1Factory : IWpfTextViewCreationListener
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("TextAdornment1")]
        [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
        [TextViewRole(PredefinedTextViewRoles.Document)]
        public AdornmentLayerDefinition EditorAdornmentLayer;

        public void TextViewCreated(IWpfTextView textView)
        {
            WindsorContainer container = ContainerFactory.CreateContainer();

            var methodsExtractor = container.Resolve<IMethodsExtractor>();
            var complexityCalculator = container.Resolve<IComplexityCalculator>();
            new TextAdornment1(textView, methodsExtractor, complexityCalculator);
        }
    }
}