using System.Drawing;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;

namespace CodeMetrics.Options
{
    [Guid(GuidList.CodeMetricsOptionsPage)]
    public class GeneralOptionsPage : DialogPage
    {
        private const string SettingsCollectionName = "Code Metrics";

        private const string ThresholdName = "Threshold";
        private const string MinimumColorName = "MinimumColor";

        public static readonly string DefaultMinColor = ColorTranslator.ToHtml(Color.Red);

        private const int DefaultThreshold = 1;

        private ShellSettingsManager settingsManager;

        private WritableSettingsStore userSettingsStore;

        public int Threshold { get; set; }

        public Color MinimumColor { get; set; }

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

        public GeneralOptionsPage()
        {
            settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
        }

        public override void ResetSettings()
        {
            userSettingsStore.SetInt32(SettingsCollectionName, ThresholdName, DefaultThreshold);
            userSettingsStore.SetString(SettingsCollectionName, MinimumColorName, DefaultMinColor);
        }


        public override void LoadSettingsFromStorage()
        {
            var installed = userSettingsStore.CollectionExists(SettingsCollectionName);
            if (!installed)
            {
                userSettingsStore.CreateCollection(SettingsCollectionName);
                ResetSettings();
            }

            Threshold = userSettingsStore.GetInt32(SettingsCollectionName, ThresholdName);
            string minimumColor = this.userSettingsStore.GetString(SettingsCollectionName, MinimumColorName);
            MinimumColor = ColorTranslator.FromHtml(minimumColor);
        }

        public override void SaveSettingsToStorage()
        {
            userSettingsStore.SetInt32(SettingsCollectionName, ThresholdName, Threshold);
            var newMinColor = ColorTranslator.ToHtml(MinimumColor);
            userSettingsStore.SetString(SettingsCollectionName, MinimumColorName, newMinColor);
        }
    }
}
