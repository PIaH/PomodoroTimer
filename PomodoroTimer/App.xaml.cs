using MvvmCommons.Core;
using PomodoroTimer.Configuration;
using PomodoroTimer.ViewModels;
using PomodoroTimer.Views;
using System.Windows;
using WPFLocalizeExtension.Engine;

namespace PomodoroTimer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            LocalizeDictionary.Instance.Culture = new System.Globalization.CultureInfo(Settings.Instance.Load().Language.Code);
            Navigator.Show<MainWindow>(new MainViewModel());

            base.OnStartup(e);
        }
    }
}
