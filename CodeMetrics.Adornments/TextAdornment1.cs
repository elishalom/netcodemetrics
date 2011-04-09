using System.Windows.Controls;
using System.Windows.Media;
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

        public TextAdornment1(IWpfTextView view)
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
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            layer.RemoveAllAdornments();

            var methodsExtractor = new MethodsExtractor();
            var methods = methodsExtractor.Extract(view.TextSnapshot.GetText());

            var textViewLines = e.NewSnapshot.Lines;
            
            foreach (var method in methods)
            {
                var line = textViewLines.ElementAt(method.Start.Line);
                int startPosition = line.Start.Position + method.Start.Column;

                SnapshotSpan span = new SnapshotSpan(view.TextSnapshot, Span.FromBounds(startPosition, startPosition + 1));
                Geometry g = view.TextViewLines.GetMarkerGeometry(span);
                if (g != null)
                {
                    GeometryDrawing drawing = new GeometryDrawing(brush, pen, g);
                    drawing.Freeze();

                    DrawingImage drawingImage = new DrawingImage(drawing);
                    drawingImage.Freeze();

                    Image image = new Image();
                    image.Source = drawingImage;

                    //Align the image with the top of the bounds of the text geometry
                    Canvas.SetLeft(image, g.Bounds.Left);
                    Canvas.SetTop(image, g.Bounds.Top);

                    layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, image, null);
                }
            }
        }
    }
}