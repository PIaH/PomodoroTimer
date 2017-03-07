using PomodoroTimer.Serialization;
using System;
using System.IO;
using WPFLocalizeExtension.Engine;

namespace PomodoroTimer.Configuration
{
    public class Settings
    {
        private const string FILENAME = "settings.xml";
        public event EventHandler SettingsChanged;

        private Settings()
        {

        }

        private static Settings instance;
        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Settings();
                }
                return instance;
            }
        }

        public SettingsEntity Load()
        {
            var dir = GetDirectory();
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filename = Path.Combine(dir, FILENAME);
            if (File.Exists(filename))
            {
                return Serializer.ReadFromXmlFile<SettingsEntity>(filename);
            }
            else
            {
                return new SettingsEntity();
            }
        }

        public void Save(SettingsEntity settings)
        {
            var dir = GetDirectory();
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filename = Path.Combine(dir, FILENAME);

            Serializer.WriteToXmlFile(filename, settings, false);
            LocalizeDictionary.Instance.Culture = new System.Globalization.CultureInfo(settings.Language.Code);
            if (SettingsChanged != null)
            {
                SettingsChanged(this, EventArgs.Empty);
            }
        }


        private string GetDirectory()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(documents, "PomodoroTimer");
        }
    }
}
