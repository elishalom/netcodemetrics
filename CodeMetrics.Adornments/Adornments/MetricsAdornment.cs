using System;
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
            this.view.TextBuffer.PostChanged += OnTextBufferChanged;

            this.methodsExtractor = methodsExtractor;
            this.complexityCalculator = complexityCalculator;

            Init(view.TextSnapshot);
        }

        private void OnTextBufferChanged(object sender, EventArgs eventArgs)
        {
            var textSnapshot = view.TextSnapshot;
            Init(textSnapshot);
        }

        private void Init(ITextSnapshot textSnapshot)
        {
            IEnumerable<IMethod> methods = methodsExtractor.Extract(textSnapshot.GetText())
                .Where(method => method.BodyEnd.Line >= 0);

            methodToText = methods.ToDictionary(method => method, method => GetText(textSnapshot, method));
        }

        private string GetText(ITextSnapshot textSnapshot, IMethod method)
        {
            return textSnapshot.GetText(GetMethodSpan(method));
        }

        private Span GetMethodSpan(IMethod method)
        {
            var startLine = view.TextSnapshot.GetLineFromLineNumber(method.BodyStart.Line);
            var endLine = view.TextSnapshot.GetLineFromLineNumber(method.BodyEnd.Line);

            int startPosition = startLine.Start.Position + method.BodyStart.Column;
            int endPosition = endLine.Start.Position + method.BodyEnd.Column;

            return Span.FromBounds(startPosition, endPosition);
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            var isVisibleAreaChanged = IsVisibleAreaChanged(e);
            if (isVisibleAreaChanged && e.OldSnapshot != e.NewSnapshot)
            {
                return;
            }
            var textSnapshot = view.TextSnapshot;

            RepaintComplexity(textSnapshot);
        }

        private static bool IsVisibleAreaChanged(TextViewLayoutChangedEventArgs e)
        {
            ViewState newState = e.NewViewState;
            ViewState oldState = e.OldViewState;
            bool isVisibleAreaChanged = newState.ViewportBottom == oldState.ViewportBottom &&
                                        newState.ViewportHeight == oldState.ViewportHeight &&
                                        newState.ViewportLeft == oldState.ViewportLeft &&
                                        newState.ViewportRight == oldState.ViewportRight &&
                                        newState.ViewportTop == oldState.ViewportTop &&
                                        newState.ViewportWidth == oldState.ViewportWidth;
            return isVisibleAreaChanged;
        }

        private void RepaintComplexity(ITextSnapshot textSnapshot)
        {
            layer.RemoveAllAdornments();

            foreach (var pair in methodToText)
            {
                var geometry = view.TextViewLines.GetMarkerGeometry(textSnapshot.GetLineFromLineNumber(pair.Key.Decleration.Line).Extent);
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