using System.Drawing;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;

namespace CodeMetrics.Options
{
    internal class Options : IOptions
    {
        private const string SettingsCollectionName = "Code Metrics";

        private const string MinimumToShowName = "MinimumToShow";
        private const string GoodColorName = "GoodColor";
        private const string BadColorName = "BadColor";

        public static readonly string DefaultGoodColor = ColorTranslator.ToHtml(Color.DarkGreen);
        public static readonly string DefaultBadColor = ColorTranslator.ToHtml(Color.Red);

        private const int DefaultToShow = 1;

        private ShellSettingsManager settingsManager;

        private readonly WritableSettingsStore userSettingsStore;

        public int MinimumToShow { get; set; }

        public Color GoodColor { get; set; }

        public Color BadColor { get; set; }

        public Options()
        {
            settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
        }

        internal void ResetSettings()
        {
            userSettingsStore.SetInt32(SettingsCollectionName, MinimumToShowName, DefaultToShow);
            userSettingsStore.SetString(SettingsCollectionName, GoodColorName, DefaultGoodColor);
            userSettingsStore.SetString(SettingsCollectionName, BadColorName, DefaultBadColor);
        }

        internal void LoadSettingsFromStorage()
        {
            var installed = userSettingsStore.CollectionExists(SettingsCollectionName);
            if (!installed)
            {
                userSettingsStore.CreateCollection(SettingsCollectionName);
                ResetSettings();
            }

            this.MinimumToShow = userSettingsStore.GetInt32(SettingsCollectionName, MinimumToShowName, DefaultToShow);
            this.GoodColor = this.ResolveSettingsColor(GoodColorName, DefaultGoodColor);
            this.BadColor = this.ResolveSettingsColor(BadColorName, DefaultBadColor);
        }

        private Color ResolveSettingsColor(string minimumColorName, string defaultColor)
        {
            var minimumColor = this.userSettingsStore.GetString(SettingsCollectionName, minimumColorName, defaultColor);
            return ColorTranslator.FromHtml(minimumColor);
        }

        internal void SaveSettingsToStorage()
        {
            userSettingsStore.SetInt32(SettingsCollectionName, MinimumToShowName, this.MinimumToShow);
            this.SaveColor(this.GoodColor, GoodColorName);
            this.SaveColor(this.BadColor, BadColorName);
        }

        private void SaveColor(Color newColor, string settingKey)
        {
            var newMinColor = ColorTranslator.ToHtml(newColor);
            this.userSettingsStore.SetString(SettingsCollectionName, settingKey, newMinColor);
        }
    }
}
