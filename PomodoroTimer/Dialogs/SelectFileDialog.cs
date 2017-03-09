using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PomodoroTimer.Dialogs
{
    public class SelectFileDialog
    {
        #region Ctor

        public SelectFileDialog()
        {
            FileExtensionsFilter = new List<string>();
            CheckIfExists = false;
            StartupDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            Title = string.Empty;
        }

        #endregion

        #region Public Interface

        public bool SelectFile(out string selectedFile)
        {
            var dlg = new OpenFileDialog();
            dlg.CheckFileExists = CheckIfExists;
            dlg.CheckPathExists = CheckIfExists;
            dlg.Filter = CreateFilter();
            dlg.DereferenceLinks = false;
            dlg.InitialDirectory = StartupDirectory;
            dlg.Multiselect = false;
            dlg.Title = Title;

            var result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                selectedFile = dlg.FileName;
                return true;
            }
            selectedFile = null;
            return false;
        }

        #endregion

        #region Properties

        public List<string> FileExtensionsFilter { get; set; }
        public bool CheckIfExists { get; set; }
        public string StartupDirectory { get; set; }
        public string Title { get; set; }

        #endregion

        #region Private Helper

        private string CreateFilter()
        {
            if (FileExtensionsFilter.Count > 0)
            {
                return string.Join("|", FileExtensionsFilter.Select(ext => "*" + ext + "|*" + ext).ToList());
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
