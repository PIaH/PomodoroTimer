using MvvmCommons.Core;
using PomodoroTimer.Configuration;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace PomodoroTimer.ViewModels
{
    public class SettingsViewModel : ModelBase
    {
        #region Ctor

        public SettingsViewModel()
        {
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            SelectSoundCommand = new RelayCommand(SelectSound);
            SelectExternalProgramCommand = new RelayCommand(SelectExternalProgram);

            var settings = Settings.Instance.Load();
            PomodoroMinutes = settings.PomodoroMinutes;
            PomodoroSeconds = settings.PomodoroSeconds;

            ShortBreakMinutes = settings.ShortBreakMinutes;
            ShortBreakSeconds = settings.ShortBreakSeconds;

            LongBreakMinutes = settings.LongBreakMinutes;
            LongBreakSeconds = settings.LongBreakSeconds;


            Languages = new ObservableCollection<SupportedLanguage>();
            Languages.Add(SupportedLanguages.German);
            Languages.Add(SupportedLanguages.English);
            Language = Languages.FirstOrDefault(l => l.Equals(settings.Language));

            SelectedSound = settings.Sound;
            SelectedExternalProgram = settings.ExternalProgram;
        }

        #endregion

        #region Properties

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SelectSoundCommand { get; set; }
        public ICommand SelectExternalProgramCommand { get; set; }

        private int pomodoroMinutes;
        public int PomodoroMinutes
        {
            get
            {
                return pomodoroMinutes;
            }
            set
            {
                pomodoroMinutes = value;
                NotifyPropertyChanged("PomodoroMinutes");

            }
        }

        private int shortBreakMinutes;
        public int ShortBreakMinutes
        {
            get
            {
                return shortBreakMinutes;
            }
            set
            {
                shortBreakMinutes = value;
                NotifyPropertyChanged("ShortBreakMinutes");
            }
        }

        private int longBreakMinutes;
        public int LongBreakMinutes
        {
            get
            {
                return longBreakMinutes;
            }
            set
            {
                longBreakMinutes = value;
                NotifyPropertyChanged("LongBreakMinutes");
            }
        }

        private int pomodoroSeconds;
        public int PomodoroSeconds
        {
            get
            {
                return pomodoroSeconds;
            }
            set
            {
                pomodoroSeconds = value;
                NotifyPropertyChanged("PomodoroSeconds");

            }
        }

        private int shortBreakSeconds;
        public int ShortBreakSeconds
        {
            get
            {
                return shortBreakSeconds;
            }
            set
            {
                shortBreakSeconds = value;
                NotifyPropertyChanged("ShortBreakSeconds");
            }
        }

        private int longBreakSeconds;
        public int LongBreakSeconds
        {
            get
            {
                return longBreakSeconds;
            }
            set
            {
                longBreakSeconds = value;
                NotifyPropertyChanged("LongBreakSeconds");
            }
        }

        private SupportedLanguage language;
        public SupportedLanguage Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
                NotifyPropertyChanged("Language");
            }
        }

        private string selectedExternalProgram;
        public string SelectedExternalProgram
        {
            get
            {
                return selectedExternalProgram;
            }
            set
            {
                selectedExternalProgram = value;
                NotifyPropertyChanged("SelectedExternalProgram");
            }
        }

        private string selectedSound;
        public string SelectedSound
        {
            get
            {
                return selectedSound;
            }
            set
            {
                selectedSound = value;
                NotifyPropertyChanged("SelectedSound");
            }
        }

        public ObservableCollection<SupportedLanguage> Languages { get; set; }

        #endregion

        #region Private Helpers

        private void Cancel(object obj)
        {
            Navigator.Back();
        }

        private void Save(object obj)
        {
            Settings.Instance.Save(new SettingsEntity
            {
                Language = this.Language,
                LongBreakMinutes = this.LongBreakMinutes,
                LongBreakSeconds = this.LongBreakSeconds,
                ShortBreakMinutes = this.ShortBreakMinutes,
                ShortBreakSeconds = this.ShortBreakSeconds,
                PomodoroMinutes = this.PomodoroMinutes,
                PomodoroSeconds = this.PomodoroSeconds,
                ExternalProgram = this.SelectedExternalProgram,
                Sound = this.SelectedSound
            });
            Navigator.Back();
        }

        private void SelectExternalProgram(object obj)
        {

        }

        private void SelectSound(object obj)
        {

        }

        #endregion
    }
}
