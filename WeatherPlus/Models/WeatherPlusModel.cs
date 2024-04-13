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

        public string MessageAdvanced
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


        public int RainAmount
        {
            get;
            set;
        }
        public bool RainOverride
        {
            get;
            set;
        } = false;
        public int CloudsAmount
        {
            get;
            set;
        }
        public bool CloudsOverride
        {
            get;
            set;
        } = false;

    }
}
