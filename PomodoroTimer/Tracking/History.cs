using PomodoroTimer.Serialization;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PomodoroTimer.Tracking
{
    public class History : IDisposable
    {
        private const string HISTORY_FILENAME = "history.xml";

        public History()
        {
            var dir = GetDirectory();
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filename = Path.Combine(dir, HISTORY_FILENAME);
            if (File.Exists(filename))
            {
                Entries = Serializer.ReadFromXmlFile<List<HistoryEntry>>(filename);
            }
            else
            {
                Entries = new List<HistoryEntry>();
            }
        }

        public int SuccessfullUnitsOfDay(DateTime date)
        {
            return Entries
                .Where(e => e.Timestamp.Date == date.Date)
                .Where(e => e.WasSuccessfull)
                .Where(e => e.Type.Equals("POMODORO"))
                .Count();
        }

        public void Save()
        {
            var dir = GetDirectory();
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filename = Path.Combine(dir, HISTORY_FILENAME);

            Serializer.WriteToXmlFile(filename, Entries, false);
        }

        public void AddEntry(HistoryEntry entry)
        {
            Entries.Add(entry);
        }

        private string GetDirectory()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(documents, "PomodoroTimer");
        }

        public void OpenHistory()
        {
            var filename = Path.GetTempFileName() + ".html";
            File.WriteAllText(filename, RenderToHtml());
            Process.Start(filename);
        }

        private List<HistoryEntry> Entries { get; set; }

        private string RenderToHtml()
        {
            string template = Properties.Resources.Template;
            return Engine.Razor.RunCompile(template, "key", Entries.GetType(), Entries);
        }

        private bool isDisposed = false;
        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                this.Save();
            }
        }
    }
}
