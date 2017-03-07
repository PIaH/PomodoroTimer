using MvvmCommons.Core;
using PomodoroTimer.Tracking;
using System;
using System.Windows.Forms;

namespace PomodoroTimer.ViewModels
{

    public class TickingViewModel : ModelBase
    {
        #region Fields

        private TimeSpan maxTimeSpan;
        private System.Timers.Timer timer;
        private string name;

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

        #endregion
    }
}
