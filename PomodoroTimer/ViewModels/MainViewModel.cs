using MvvmCommons.Core;
using PomodoroTimer.Configuration;
using PomodoroTimer.Tracking;
using PomodoroTimer.Views;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PomodoroTimer.ViewModels
{
    public class MainViewModel : ModelBase
    {
        #region Ctor

        public MainViewModel()
        {

            PomodoroViewModel = new TickingViewModel("POMODORO", TimeSpan.FromMinutes(25));
            PomodoroViewModel.IsActive = true;
            SmallBreakViewModel = new TickingViewModel("SHORT_BREAK", TimeSpan.FromMinutes(3));
            BigBreakViewModel = new TickingViewModel("LONG_BREAK", TimeSpan.FromMinutes(30));
            PomodoroViewModel.UpdateProgressValue += UpdateTaskbarProgress;
            SmallBreakViewModel.UpdateProgressValue += UpdateTaskbarProgress;
            BigBreakViewModel.UpdateProgressValue += UpdateTaskbarProgress;

            SettingsChanged(null, null); // This will load the configured timespans and updates topmost

            Predicate<object> nothingIsTicking = (o) =>
            {
                return !PomodoroViewModel.IsTicking && !SmallBreakViewModel.IsTicking && !BigBreakViewModel.IsTicking;
            };

            Predicate<object> anyoneIsTicking = (o) =>
            {
                return PomodoroViewModel.IsTicking || SmallBreakViewModel.IsTicking || BigBreakViewModel.IsTicking;
            };
            StartPomodoroCommand = new RelayCommand(StartPomodoro, nothingIsTicking);
            StartSmallBreakCommand = new RelayCommand(StartSmallBreak, nothingIsTicking);
            StartBigBreakCommand = new RelayCommand(StartBigBreak, nothingIsTicking);
            InterruptCommand = new RelayCommand(Interrupt, anyoneIsTicking);
            ShowHistoryCommand = new RelayCommand(ShowHistory);
            ShowSettingsCommand = new RelayCommand(ShowSettings, nothingIsTicking);
            MinimizeCommand = new RelayCommand(Minimize);
            QuitCommand = new RelayCommand(QuitApplication);

            Settings.Instance.SettingsChanged += SettingsChanged;

            ItemInfo = new System.Windows.Shell.TaskbarItemInfo();
            ItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal;
            ItemInfo.ProgressValue = 0;
        }

        private void QuitApplication(object obj)
        {
            if (InterruptCommand.CanExecute(null))
            {
                InterruptCommand.Execute(null);
            }
            Application.Current.Shutdown();
        }

        private void Minimize(object obj)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Minimized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            }
        }

        #endregion

        #region Properties

        public ICommand StartPomodoroCommand { get; set; }

        public ICommand StartSmallBreakCommand { get; set; }

        public ICommand StartBigBreakCommand { get; set; }

        public ICommand InterruptCommand { get; set; }

        public ICommand ShowHistoryCommand { get; set; }

        public ICommand ShowSettingsCommand { get; set; }

        public ICommand MinimizeCommand { get; set; }

        public ICommand QuitCommand { get; set; }


        public TickingViewModel BigBreakViewModel { get; set; }

        public TickingViewModel SmallBreakViewModel { get; set; }

        public TickingViewModel PomodoroViewModel { get; set; }

        public System.Windows.Shell.TaskbarItemInfo ItemInfo { get; set; }

        private bool isTopMost;
        public bool IsTopMost
        {
            get
            {
                return isTopMost;
            }
            set
            {
                isTopMost = value;
                NotifyPropertyChanged("IsTopMost");
            }
        }



        #endregion

        #region Private Helper

        private void SettingsChanged(object sender, EventArgs e)
        {
            var settings = Settings.Instance.Load();
            PomodoroViewModel.UpdateMaxTimeSpan(TimeSpan.FromMinutes(settings.PomodoroMinutes).Add(TimeSpan.FromSeconds(settings.PomodoroSeconds)));
            BigBreakViewModel.UpdateMaxTimeSpan(TimeSpan.FromMinutes(settings.LongBreakMinutes).Add(TimeSpan.FromSeconds(settings.LongBreakSeconds)));
            SmallBreakViewModel.UpdateMaxTimeSpan(TimeSpan.FromMinutes(settings.ShortBreakMinutes).Add(TimeSpan.FromSeconds(settings.ShortBreakSeconds)));
            IsTopMost = settings.IsTopMost;
        }

        private void Interrupt(object o = null)
        {
            if (BigBreakViewModel.IsActive)
            {
                BigBreakViewModel.Interrupt();
            }
            if (SmallBreakViewModel.IsActive)
            {
                SmallBreakViewModel.Interrupt();
            }
            if (PomodoroViewModel.IsActive)
            {
                PomodoroViewModel.Interrupt();
            }
        }

        private void StartPomodoro(object o = null)
        {
            SmallBreakViewModel.Deactivate();
            BigBreakViewModel.Deactivate();
            PomodoroViewModel.Start();
        }

        private void StartSmallBreak(object o = null)
        {
            PomodoroViewModel.Deactivate();
            BigBreakViewModel.Deactivate();
            SmallBreakViewModel.Start();
        }

        private void StartBigBreak(object o = null)
        {
            PomodoroViewModel.Deactivate();
            SmallBreakViewModel.Deactivate();
            BigBreakViewModel.Start();
        }

        private void ShowHistory(object obj)
        {
            using (var history = new History())
            {
                history.OpenHistory();
            }
        }

        private void ShowSettings(object obj)
        {
            Navigator.Show<SettingsWindow>(new SettingsViewModel(), true);
        }

        private void UpdateTaskbarProgress(double progress)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ItemInfo.ProgressValue = progress;
            });
        }

        #endregion
    }
}
