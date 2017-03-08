using MvvmCommons.Core;
using PomodoroTimer.Configuration;
using PomodoroTimer.Tracking;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using WMPLib;

namespace PomodoroTimer.ViewModels
{

    public class TickingViewModel : ModelBase
    {
        #region Fields

        private TimeSpan maxTimeSpan;
        private System.Timers.Timer timer;
        private string name;

        public event Action<double> UpdateProgressValue;

        #endregion

        #region Ctor

        public TickingViewModel(string name, TimeSpan maxTimeSpan)
        {
            this.name = name;
            this.maxTimeSpan = maxTimeSpan;
            this.timer = new System.Timers.Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            this.timer.AutoReset = true;
            this.timer.Elapsed += delegate
            {
                this.RemainingTimeSpan -= TimeSpan.FromSeconds(1);
                double percentageValue = this.remainingTimeSpan.TotalSeconds / this.maxTimeSpan.TotalSeconds;
                FireUpdateProgressValue(percentageValue);
                if (this.RemainingTimeSpan == TimeSpan.FromSeconds(0) || this.RemainingTimeSpan < TimeSpan.FromSeconds(1))
                {
                    this.timer.Enabled = false;
                    this.Finished();
                    this.IsTicking = false;
                    this.RemainingTimeSpan = this.maxTimeSpan;
                }
            };
            this.RemainingTimeSpan = this.maxTimeSpan;
            UpdateUnitsOfToday();
        }

        #endregion

        #region Public Interface

        public void Activate()
        {
            this.IsActive = true;
        }

        public void Deactivate()
        {
            this.IsActive = false;
        }

        public void Interrupt()
        {
            this.timer.Enabled = false;
            this.IsTicking = false;
            this.RemainingTimeSpan = this.maxTimeSpan;
            this.Finished(false);
        }

        public void Start()
        {
            if (this.timer.Enabled == false)
            {
                FireUpdateProgressValue(1);
                this.Activate();
                this.RemainingTimeSpan = this.maxTimeSpan;
                this.timer.Enabled = true;
                this.IsTicking = true;
            }
        }

        public void UpdateMaxTimeSpan(TimeSpan maxTimeSpan)
        {
            if (!IsTicking)
            {
                this.maxTimeSpan = maxTimeSpan;
                this.RemainingTimeSpan = maxTimeSpan;
            }
        }

        #endregion

        #region Properties

        private TimeSpan remainingTimeSpan;

        public TimeSpan RemainingTimeSpan
        {
            get
            {
                return remainingTimeSpan;
            }
            set
            {
                remainingTimeSpan = value;
                NotifyPropertyChanged("RemainingTimeSpan");
            }
        }

        private bool isTicking;
        public bool IsTicking
        {
            get
            {
                return isTicking;
            }
            set
            {
                isTicking = value;
                NotifyPropertyChanged("IsTicking");
            }
        }

        private bool isActive;
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
                NotifyPropertyChanged("IsActive");
            }
        }

        private int successfullUnitsOfToday;
        public int SuccessfullUnitsOfToday
        {
            get
            {
                return successfullUnitsOfToday;
            }
            set
            {
                successfullUnitsOfToday = value;
                NotifyPropertyChanged("SuccessfullUnitsOfToday");
            }
        }

        #endregion

        #region Private Helpers

        private void Finished(bool success = true)
        {
            if (success)
            {
                var settings = Settings.Instance.Load();
                // Play sound if configured
                if (settings.Sound != null)
                {
                    var player = new WindowsMediaPlayer();
                    player.URL = settings.Sound;
                    player.controls.currentPosition = 0;
                    player.controls.play();
                }
                // Start external program if configured
                if (settings.ExternalProgram != null)
                {
                    Process.Start(settings.ExternalProgram);
                }
            }
            FireUpdateProgressValue(0);
            var finished = WPFLocalizeExtension.Extensions.LocTextExtension.GetLocalizedValue<string>("FINISHED");
            var interrupted = WPFLocalizeExtension.Extensions.LocTextExtension.GetLocalizedValue<string>("INTERRUPTED");
            var namevalue = WPFLocalizeExtension.Extensions.LocTextExtension.GetLocalizedValue<string>(name);

            using (var history = new History())
            {
                history.AddEntry(new HistoryEntry
                {
                    DurationInMs = Convert.ToInt32(this.maxTimeSpan.TotalMilliseconds),
                    Name = namevalue,
                    Type = this.name,
                    Timestamp = DateTime.UtcNow,
                    WasSuccessfull = success
                });
            }

            var icon = new NotifyIcon();
            icon.Icon = Properties.Resources.pomodoro;
            icon.Visible = true;
            icon.ShowBalloonTip(500, "PomodoroTimer", namevalue + " " + (success ? finished : interrupted), ToolTipIcon.None);
            UpdateUnitsOfToday();
        }

        private void UpdateUnitsOfToday()
        {
            using (var history = new History())
            {
                SuccessfullUnitsOfToday = history.SuccessfullUnitsOfDay(DateTime.UtcNow);
            }
        }

        private void FireUpdateProgressValue(double value)
        {
            if (UpdateProgressValue != null)
            {
                UpdateProgressValue(value);
            }
        }
        #endregion
    }
}
