using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using CodeMetrics.Calculators;

namespace CodeMetrics.Addin
{
    public class ComplexityViewHost : UserControl
    {
        private ElementHost wpfHost;

        public ComplexityViewHost(IComplexity complexity)
        {
            InitializeComponent(complexity);
        }

        private void InitializeComponent(IComplexity complexity)
        {
            Size = new Size(30, 20);
            wpfHost = new ElementHost
                          {
                              Dock = DockStyle.Fill,
                              Child = new ComplexityView(complexity)
                          };
            Controls.Add(wpfHost);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            wpfHost.Refresh();
            base.OnPaint(e);
        }
    }
}
