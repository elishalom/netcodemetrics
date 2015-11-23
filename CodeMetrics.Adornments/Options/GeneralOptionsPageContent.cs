using System;
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
            textBox1.Text = this.OptionsPage.OptionString;
        }

        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            this.OptionsPage.OptionString = textBox1.Text;
        }
    }
}
