using System;
using System.Globalization;
using System.Windows.Forms;
using CodeMetrics.UserControls;

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
            this.minimumColorSelection.SelectedColor = this.OptionsPage.GoodColor;
            this.maximumColorSelection.SelectedColor = this.OptionsPage.BadColor;
        }

        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            int newValue = 1;
            if (int.TryParse(this.thresholdTextbox.Text, out newValue))
            {
                this.OptionsPage.Threshold = newValue;
                this.thresholdErrorProvider.SetError(this.thresholdTextbox, string.Empty);
            }
            else
            {
                this.thresholdErrorProvider.SetError(this.thresholdTextbox, Properties.Resources.ThresholdErrorMessage);
            }
        }

        private void MinimumColorSelection_SelectedColorChanged(object sender, ColorChangedEventArgs e)
        {
            this.OptionsPage.GoodColor = e.NewColor;
        }

        private void MaximumColorSelection_SelectedColorChanged(object sender, ColorChangedEventArgs e)
        {
            this.OptionsPage.BadColor = e.NewColor;
        }
    }
}
