using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeMetrics.Options
{
    internal partial class ColorSelection : UserControl
    {
        public event EventHandler<ColorChangedEventArgs> SelectedColorChanged; 

        internal Color SelectedColor
        {
            get
            {
                return this.previewPanel.BackColor;
            }
            set
            {
                this.previewPanel.BackColor = value;
            }
        }

        public ColorSelection()
        {
            InitializeComponent();
        }

        private void ChangeColorButtonClick(object sender, EventArgs e)
        {
            this.rangeColorDialog.Color = this.SelectedColor;

            if (this.rangeColorDialog.ShowDialog() == DialogResult.OK)
            {
                this.SelectedColor = this.rangeColorDialog.Color;
                FireColorChanged();
            }
        }

        private void FireColorChanged()
        {
            if (SelectedColorChanged != null)
            {
                var args = new ColorChangedEventArgs() { NewColor = this.SelectedColor };
                SelectedColorChanged(this, args);
            }
        }
    }
}
