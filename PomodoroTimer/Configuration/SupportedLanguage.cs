
namespace PomodoroTimer.Configuration
{
    public class SupportedLanguage
    {
        private string name;

        // Required for Serialization
        public SupportedLanguage()
        {

        }

        public SupportedLanguage(string name, string code)
        {
            this.name = name;
            this.Code = code;
        }

        public string Code { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is SupportedLanguage)
            {
                var lang = obj as SupportedLanguage;
                if (lang != null && lang.Code != null)
                {
                    return (lang.Code.Equals(this.Code));
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode() + this.Code.GetHashCode();
        }

        public override string ToString()
        {
            return name;
        }
    }
}
