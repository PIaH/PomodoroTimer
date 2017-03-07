
namespace PomodoroTimer.Configuration
{
    public class SettingsEntity
    {
        public SettingsEntity()
        {
            Language = SupportedLanguages.English;
            PomodoroMinutes = 25;
            PomodoroSeconds = 0;
            ShortBreakMinutes = 3;
            ShortBreakSeconds = 0;
            LongBreakMinutes = 30;
            LongBreakSeconds = 0;
        }

        public SupportedLanguage Language { get; set; }
        public int PomodoroMinutes { get; set; }
        public int PomodoroSeconds { get; set; }
        public int ShortBreakMinutes { get; set; }
        public int ShortBreakSeconds { get; set; }
        public int LongBreakMinutes { get; set; }
        public int LongBreakSeconds { get; set; }
    }
}
