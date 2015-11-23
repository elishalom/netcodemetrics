using System;
using System.Globalization;
using System.Windows.Forms;

namespace CodeMetrics.Options
{
    public partial class GeneralOptionsPageContent : UserControl
    {
        internal GeneralOptionsPage OptionsPage { get; set; }

        public GeneralOptionsPageContent()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            this.thresholdTextbox.Text = this.OptionsPage.Threshold.ToString(CultureInfo.InvariantCulture);
            this.minimumColorPreview.BackColor = this.OptionsPage.MinimumColor;
        }

        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            int newValue = 1;
            if (int.TryParse(this.thresholdTextbox.Text, out newValue))
                this.OptionsPage.Threshold = newValue;
        }

        private void MinimumColorButtonClick(object sender, EventArgs e)
        {
            if (this.rangeColorDialog.ShowDialog() == DialogResult.OK)
            {
                this.minimumColorPreview.BackColor = this.rangeColorDialog.Color;
                this.OptionsPage.MinimumColor = this.rangeColorDialog.Color;
            }
        }
    }
}
