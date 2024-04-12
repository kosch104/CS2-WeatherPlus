using Gooee.Plugins;

namespace WeatherPlus.Models
{

   
    public class WeatherPlusModel : Model
    {
        public bool IsVisible
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        } = "Using Default Settings";

        public string MessageRain
        {
            get;
            set;
        } = "Using Default Settings";

        public int Temperature
        {
            get;
            set;
        }

        public bool TemperatureOverride
        {
            get;
            set;
        } = false;
    }
}
