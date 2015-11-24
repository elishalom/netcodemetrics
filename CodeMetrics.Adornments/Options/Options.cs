using System.Drawing;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;

namespace CodeMetrics.Options
{
    internal class Options : IOptions
    {
        private const string SettingsCollectionName = "Code Metrics";

        private const string ThresholdName = "Threshold";
        private const string MinimumColorName = "MinimumColor";
        private const string MaximumColorName = "MaximumColor";

        public static readonly string DefaultMinColor = ColorTranslator.ToHtml(Color.DarkGreen);
        public static readonly string DefaultMaxColor = ColorTranslator.ToHtml(Color.Red);

        private const int DefaultThreshold = 1;

        private ShellSettingsManager settingsManager;

        private WritableSettingsStore userSettingsStore;

        public int Threshold { get; set; }

        public Color MinimumColor { get; set; }

        public Color MaximumColor { get; set; }

        public Options()
        {
            settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
        }

        internal void ResetSettings()
        {
            userSettingsStore.SetInt32(SettingsCollectionName, ThresholdName, DefaultThreshold);
            userSettingsStore.SetString(SettingsCollectionName, MinimumColorName, DefaultMinColor);
            userSettingsStore.SetString(SettingsCollectionName, MaximumColorName, DefaultMaxColor);
        }

        internal void LoadSettingsFromStorage()
        {
            var installed = userSettingsStore.CollectionExists(SettingsCollectionName);
            if (!installed)
            {
                userSettingsStore.CreateCollection(SettingsCollectionName);
                ResetSettings();
            }

            Threshold = userSettingsStore.GetInt32(SettingsCollectionName, ThresholdName);
            MinimumColor = this.ResolveSettingsColor(MinimumColorName);
            MaximumColor = this.ResolveSettingsColor(MaximumColorName);
        }

        private Color ResolveSettingsColor(string minimumColorName)
        {
            string minimumColor = this.userSettingsStore.GetString(SettingsCollectionName, minimumColorName);
            return ColorTranslator.FromHtml(minimumColor);
        }

        internal void SaveSettingsToStorage()
        {
            userSettingsStore.SetInt32(SettingsCollectionName, ThresholdName, Threshold);
            this.SaveColor(this.MinimumColor, MinimumColorName);
            this.SaveColor(this.MaximumColor, MaximumColorName);
        }

        private void SaveColor(Color newColor, string settingKey)
        {
            var newMinColor = ColorTranslator.ToHtml(newColor);
            this.userSettingsStore.SetString(SettingsCollectionName, settingKey, newMinColor);
        }
    }
}
