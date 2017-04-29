using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using CodeMetrics.Calculators.Contracts;
using CodeMetrics.Parsing.Contracts;
using CodeMetrics.UserControls;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace CodeMetrics.Adornments
{
    public class MetricsAdornment : IMetricsAdornment
    {
        private readonly IAdornmentLayer layer;
        private readonly IWpfTextView view;
        private readonly IMethodsExtractor methodsExtractor;
        private readonly ICyclomaticComplexityCalculator cyclomaticComplexityCalculator;
        private readonly Options.Options options = new Options.Options();

        private Dictionary<ISyntaxNode, ISyntaxNode> syntaxNodes;

        public MetricsAdornment(IWpfTextView view, IMethodsExtractor methodsExtractor, ICyclomaticComplexityCalculator cyclomaticComplexityCalculator)
        {
            if (methodsExtractor == null) throw new ArgumentNullException(nameof(methodsExtractor));
            if (cyclomaticComplexityCalculator == null) throw new ArgumentNullException(nameof(cyclomaticComplexityCalculator));

            this.view = view;
            layer = view.GetAdornmentLayer(MeticsAdornmentFactory.ADORNMENT_NAME);

            this.view.LayoutChanged += OnLayoutChanged;
            this.view.TextBuffer.PostChanged += OnTextBufferChanged;

            this.methodsExtractor = methodsExtractor;
            this.cyclomaticComplexityCalculator = cyclomaticComplexityCalculator;

            Init(view.TextSnapshot);
        }

        private void OnTextBufferChanged(object sender, EventArgs eventArgs)
        {
            var textSnapshot = view.TextSnapshot;
            Init(textSnapshot);
        }

        private void Init(ITextSnapshot textSnapshot)
        {
            var methods = methodsExtractor.Extract(textSnapshot.GetText())
                .OfType<ISyntaxNode>();

            syntaxNodes = methods.ToDictionary(method => method, method => method);
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
            var newState = e.NewViewState;
            var oldState = e.OldViewState;
            var isVisibleAreaChanged = newState.ViewportBottom == oldState.ViewportBottom &&
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
            options.LoadSettingsFromStorage();

            foreach (var pair in syntaxNodes)
            {
                var line = textSnapshot.GetLineFromLineNumber(pair.Key.Declaration.Line);
                var geometry = view.TextViewLines.GetMarkerGeometry(line.Extent);
                var isMethodVisible = geometry != null;
                if (!isMethodVisible)
                {
                    continue;
                }

                var complexityViewModel = new ComplexityViewModel(options);
                var complexityView = new ComplexityView
                {
                    DataContext = complexityViewModel
                };
                CreateTask(complexityViewModel, pair.Key).Start();

                Canvas.SetLeft(complexityView, geometry.Bounds.Left);
                Canvas.SetTop(complexityView, geometry.Bounds.Top);

                layer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, complexityView, null);
            }
        }

        private Task CreateTask(ComplexityViewModel complexityViewModel, ISyntaxNode syntaxNode)
        {
            return new Task(() => complexityViewModel.UpdateComplexity(cyclomaticComplexityCalculator.Calculate(syntaxNode)));
        }
    }
}