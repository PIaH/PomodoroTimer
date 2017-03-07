using System;

namespace PomodoroTimer.Tracking
{
    public class HistoryEntry
    {
        public HistoryEntry()
        {
            Name = "POMODORO";
        }

        public DateTime Timestamp { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long DurationInMs { get; set; }
        public bool WasSuccessfull { get; set; }
    }
}
