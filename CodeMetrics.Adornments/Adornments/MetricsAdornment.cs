using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using CodeMetrics.Calculators;
using CodeMetrics.Parsing;
using CodeMetrics.UserControls;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.Linq;

namespace CodeMetrics.Adornments
{
    public class MetricsAdornment : IMetricsAdornment
    {
        readonly IAdornmentLayer layer;
        readonly IWpfTextView view;
        private readonly IMethodsExtractor methodsExtractor;
        private readonly IComplexityCalculator complexityCalculator;

        private Dictionary<IMethod, string> methodToText;

        public MetricsAdornment(IWpfTextView view, IMethodsExtractor methodsExtractor, IComplexityCalculator complexityCalculator)
        {
            this.view = view;
            layer = view.GetAdornmentLayer(MeticsAdornmentFactory.ADORNMENT_NAME);

            this.view.LayoutChanged += OnLayoutChanged;
            this.view.TextBuffer.Changed += TextBufferOnChanged;

            this.methodsExtractor = methodsExtractor;
            this.complexityCalculator = complexityCalculator;

            Init(view.TextSnapshot);
        }

        private void TextBufferOnChanged(object sender, TextContentChangedEventArgs textContentChangedEventArgs)
        {
            var textSnapshot = textContentChangedEventArgs.After;
            Init(textSnapshot);
        }

        private void Init(ITextSnapshot textSnapshot)
        {
            IEnumerable<IMethod> methods = methodsExtractor.Extract(textSnapshot.GetText())
                .Where(method => method.End.Line >= 0);

            methodToText = methods.ToDictionary(method => method, method => textSnapshot.GetText(GetMethodSpan(method)));
        }

        private Span GetMethodSpan(IMethod method)
        {
            var startLine = view.TextSnapshot.GetLineFromLineNumber(method.Start.Line - 1);
            var endLine = view.TextSnapshot.GetLineFromLineNumber(method.End.Line - 1);

            int startPosition = startLine.Start.Position + method.Start.Column;
            int endPosition = endLine.Start.Position + method.End.Column;

            return Span.FromBounds(startPosition, endPosition);
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            /*if(e.NewSnapshot == e.OldSnapshot)
            {
                return;
            }*/
            var textSnapshot = view.TextSnapshot;

            RepaintComplexity(textSnapshot);
        }

        private void RepaintComplexity(ITextSnapshot textSnapshot)
        {
            /*if (view.TextSnapshot != textSnapshot)
            {
                return;
            }*/

            layer.RemoveAllAdornments();

            foreach (var pair in methodToText)
            {
                var geometry = view.TextViewLines.GetMarkerGeometry(textSnapshot.GetLineFromLineNumber(pair.Key.Start.Line).Extent);
                var isMethodVisible = geometry != null;
                if (!isMethodVisible)
                {
                    continue;
                }

                string methodText = methodToText[pair.Key];
                var complexityViewModel = new ComplexityViewModel();
                var complexityView = new ComplexityView
                                         {
                                             DataContext = complexityViewModel
                                         };
                new Task(() => complexityViewModel.UpdateComplexity(complexityCalculator.Calculate(methodText))).Start();

                Canvas.SetLeft(complexityView, geometry.Bounds.Left);
                Canvas.SetTop(complexityView, geometry.Bounds.Top);

                layer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, complexityView, null);
            }
        }
    }
}