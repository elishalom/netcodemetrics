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

        private ShellSettingsManager settingsManager;

        private WritableSettingsStore userSettingsStore;

        public string OptionString { get; set; }

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
            userSettingsStore.SetString(SettingsCollectionName, nameof(OptionString), "alpha");
        }

        public override void LoadSettingsFromStorage()
        {
            var installed = userSettingsStore.CollectionExists(SettingsCollectionName);
            if (!installed)
            {
                userSettingsStore.CreateCollection(SettingsCollectionName);
                ResetSettings();
            }

            OptionString = userSettingsStore.GetString(SettingsCollectionName, nameof(OptionString));
        }

        public override void SaveSettingsToStorage()
        {
            userSettingsStore.SetString(SettingsCollectionName, nameof(OptionString), OptionString);
        }
    }
}
