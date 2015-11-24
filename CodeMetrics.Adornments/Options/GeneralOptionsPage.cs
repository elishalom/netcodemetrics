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
                return this.options.MinimumToShow;
            }
            set
            {
                this.options.MinimumToShow = value;
            }
        }

        public Color GoodColor
        {
            get
            {
                return this.options.GoodColor;
            }
            set
            {
                this.options.GoodColor = value;
            }
        }

        public Color BadColor
        {
            get
            {
                return this.options.BadColor;
            }
            set
            {
                this.options.BadColor = value;
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
