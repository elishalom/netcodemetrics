using System.Drawing;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CodeMetrics.Options
{
    [Guid(GuidList.CodeMetricsOptionsPage)]
    public class GeneralOptionsPage : DialogPage
    {
        private readonly Options options = new Options();

        public int Threshold
        {
            get
            {
                return this.options.Threshold;
            }
            set
            {
                this.options.Threshold = value;
            }
        }

        public Color MinimumColor
        {
            get
            {
                return this.options.MinimumColor;
            }
            set
            {
                this.options.MinimumColor = value;
            }
        }

        public Color MaximumColor
        {
            get
            {
                return this.options.MaximumColor;
            }
            set
            {
                this.options.MaximumColor = value;
            }
        }

        protected override IWin32Window Window
        {
            get
            {
                var page = new GeneralOptionsPageContent();
                page.OptionsPage = this;
                page.Initialize();
                return page;
            }
        }

        public override void ResetSettings()
        {
            this.options.ResetSettings();
        }

        public override void LoadSettingsFromStorage()
        {
            this.options.LoadSettingsFromStorage();
        }

        public override void SaveSettingsToStorage()
        {
            this.options.SaveSettingsToStorage();
        }
    }
}
