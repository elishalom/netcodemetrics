using System.Windows.Controls;
using System.Windows.Media;
using CodeMetrics.Calculators;
using CodeMetrics.Parsing;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using System.Linq;

namespace TextAdornment1
{
    public class TextAdornment1
    {
        readonly IAdornmentLayer layer;
        readonly IWpfTextView view;
        readonly Brush brush;
        readonly Pen pen;
        private readonly IMethodsExtractor methodsExtractor;
        private readonly IComplexityCalculator complexityCalculator;

        public TextAdornment1(IWpfTextView view, IMethodsExtractor methodsExtractor, IComplexityCalculator complexityCalculator)
        {
            this.view = view;
            layer = view.GetAdornmentLayer("TextAdornment1");

            //Listen to any event that changes the layout (text changes, scrolling, etc)
            this.view.LayoutChanged += OnLayoutChanged;

            Brush brush = new SolidColorBrush(Color.FromArgb(0x20, 0x00, 0x00, 0xff));
            brush.Freeze();
            Brush penBrush = new SolidColorBrush(Colors.Red);
            penBrush.Freeze();
            Pen pen = new Pen(penBrush, 0.5);
            pen.Freeze();

            this.brush = brush;
            this.pen = pen;
            this.methodsExtractor = methodsExtractor;
            this.complexityCalculator = complexityCalculator;
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            layer.RemoveAllAdornments();

            var methods = methodsExtractor.Extract(view.TextSnapshot.GetText());
            
            foreach (var method in methods)
            {
                var line = view.TextSnapshot.Lines.ElementAt(method.Start.Line);
                int startPosition = line.Start.Position + method.Start.Column;
                var endLine = view.TextSnapshot.Lines.ElementAt(method.End.Line);
                int endPosition = endLine.Start.Position + method.End.Column;
                var complexity = complexityCalculator.Calculate(view.TextSnapshot.GetText(startPosition, endPosition - startPosition));

                SnapshotSpan span = new SnapshotSpan(view.TextSnapshot, Span.FromBounds(startPosition-2, startPosition -1));
                Geometry g = view.TextViewLines.GetMarkerGeometry(span);
                if (g != null)
                {
//                    GeometryDrawing drawing = new GeometryDrawing(brush, pen, g);
//                    drawing.Freeze();

//                    DrawingImage drawingImage = new DrawingImage(drawing);
//                    drawingImage.Freeze();

//                    Image image = new Image();
//                    image.Source = drawingImage;

                    var textBlock = new TextBlock() {Text = complexity.Value.ToString()};
                    textBlock.Width = 30;
                    textBlock.Height = 30;
                    textBlock.Background = Brushes.Aqua;

                    //Align the image with the top of the bounds of the text geometry
                    Canvas.SetLeft(textBlock, g.Bounds.Left);
                    Canvas.SetTop(textBlock, g.Bounds.Top);

                    layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, textBlock, null);
                }
            }
        }

        private int GetPosition(Location location)
        {
            var lineIndex = location.Line;
            var columnIndex = location.Column;
            int position = 0;
            for(int i=0;i<lineIndex; i++)
            {
                position += view.TextViewLines[i].Length;
            }
            position += columnIndex;
            return position;
        }
    }
}