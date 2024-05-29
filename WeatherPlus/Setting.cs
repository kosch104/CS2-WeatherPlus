using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using System.Collections.Generic;



namespace WeatherPlus
{
    [FileLocation(nameof(WeatherPlus))]
    [SettingsUIGroupOrder(kButtonGroup2, kSliderGroup, kDropdownGroup)]
    [SettingsUIShowGroupName(kButtonGroup2, kSliderGroup, kDropdownGroup)]
    public class Setting : ModSetting
    {

        private readonly Mod _mod;
        private DanielsWeatherSystem _weatherSystem;



        public const string kSection = "Main";
        public const string kSection2 = "Time";
        public const string kSection0 = "Main1";
        public const string kButtonGroup2 = "Button2";
        public const string kDropdownGroup = "Dropdown";
        public const string kSliderGroup = "Slider";
        public float currentTemp;
        public float currentPrecipitation;
        public float cloudiness;
        public bool enableTemperature;
        public bool enablePrecipitation;
        public bool enableCloudiness;

        public float customTime = 12f;


        public Setting(IMod mod, DanielsWeatherSystem weatherSystem) : base(mod)
        {
            _mod = (Mod)mod;
            _weatherSystem = weatherSystem;
            Mod.log.Info("Setting initialized");


            currentTemp = Temperature;
            currentPrecipitation = Precipitation;
            cloudiness = Cloudiness;
            enableTemperature = EnabableTemperature;
            enablePrecipitation = EnablePrecipitation;
            enableCloudiness = EnableCloudiness;




            _weatherSystem.UpdateWeather(currentTemp, currentPrecipitation, cloudiness);
            _weatherSystem.UpdateTimeOfDay(customTime, Default, CustomTimeBool, CustomTime);


        }

        //Page1 - Presets

        [SettingsUISection(kSection0, kButtonGroup2)]
        public bool Default { get; set; }

        [SettingsUISection(kSection0, kButtonGroup2)]
        public bool TimeSixAM
        {
            set
            {
                customTime = 5f;
                _weatherSystem.UpdateTimeOfDay(customTime, Default, CustomTimeBool, CustomTime);
            }
        }
        [SettingsUISection(kSection0, kButtonGroup2)]
        public bool TimeSevenAM
        {
            set
            {
                customTime = 6f;
                _weatherSystem.UpdateTimeOfDay(customTime, Default, CustomTimeBool, CustomTime);
            }
        }

        [SettingsUISection(kSection0, kButtonGroup2)]
        public bool Day
        {
            set
            {
                customTime = 13f;
                _weatherSystem.UpdateTimeOfDay(customTime, Default, CustomTimeBool, CustomTime);
            }
        }

        [SettingsUISection(kSection0, kButtonGroup2)]
        public bool Night
        {
            set
            {
                customTime = 22f;
                _weatherSystem.UpdateTimeOfDay(customTime, Default, CustomTimeBool, CustomTime);
            }
        }


        //Page2 - Custom Weather Information

        [SettingsUISection(kSection, kSliderGroup)]
        public bool EnabableTemperature { get; set; }

        [SettingsUISlider(min = -50, max = 50, step = 1, scalarMultiplier = 1, unit = Unit.kTemperature)]
        [SettingsUISection(kSection, kSliderGroup)]
        public float Temperature { get; set; }

        [SettingsUISection(kSection, kSliderGroup)]
        public bool EnablePrecipitation { get; set; }

        [SettingsUISlider(min = 0.000f, max = 0.999f, step = 0.001f, scalarMultiplier = 1, unit = Unit.kFloatThreeFractions)]
        [SettingsUISection(kSection, kSliderGroup)]
        public float Precipitation { get; set; }

        [SettingsUISection(kSection, kSliderGroup)]
        public bool EnableCloudiness { get; set; }

        [SettingsUISlider(min = 0.000f, max = 0.999f, step = 0.001f, scalarMultiplier = 1, unit = Unit.kFloatThreeFractions)]
        [SettingsUISection(kSection, kSliderGroup)]
        public float Cloudiness { get; set; }


        //Page3 - Time Information







        [SettingsUISection(kSection2, kDropdownGroup)]
        public bool CustomTimeBool { get; set; }


        [SettingsUISlider(min = 0, max = 23.99f, step = 0.10f, scalarMultiplier = 1, unit = Unit.kFloatTwoFractions)]
        [SettingsUISection(kSection2, kDropdownGroup)]
        public float CustomTime { get; set; }





        public override void Apply()
        {
            Mod.log.Info("Running Apply method..........");
            currentTemp = Temperature;
            currentPrecipitation = Precipitation;
            cloudiness = Cloudiness;
            enableTemperature = EnabableTemperature;
            enablePrecipitation = EnablePrecipitation;
            enableCloudiness = EnableCloudiness;

            _weatherSystem.UpdateWeather(currentTemp, currentPrecipitation, cloudiness);
            _weatherSystem.UpdateTimeOfDay(customTime, Default, CustomTimeBool, CustomTime);

            Mod.log.Info("Weather updated successfully from Apply method.");






        }


        public override void SetDefaults()
        {
            Mod.log.Info("SetDefaults Ran Successfully");

        }
    }

    public class LocaleEN : IDictionarySource
    {
        private readonly Setting _setting;
        public LocaleEN(Setting setting)
        {
            _setting = setting;
        }
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { _setting.GetSettingsLocaleID(), "Weather+" },
                { _setting.GetOptionTabLocaleID(Setting.kSection), "Weather Settings" },
                { _setting.GetOptionTabLocaleID(Setting.kSection2), "Time Settings" },
                { _setting.GetOptionTabLocaleID(Setting.kSection0), "Presets" },
                { _setting.GetOptionGroupLocaleID(Setting.kButtonGroup2), "Presets" },
                { _setting.GetOptionGroupLocaleID(Setting.kSliderGroup), "Change Current Weather" },
                { _setting.GetOptionDescLocaleID(Setting.kSection), "Change the current weather settings." },
                { _setting.GetOptionGroupLocaleID(Setting.kDropdownGroup), "Choose a Custom Time - Enable Custom Settings under PRESETS must be TICKED" },


                { _setting.GetOptionLabelLocaleID(nameof(Setting.TimeSixAM)), "5:00AM" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.TimeSixAM)), $"Sets the time to 6:00AM" },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.TimeSevenAM)), "6:00AM" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.TimeSevenAM)), $"Sets the time to 7:00AM" },


                { _setting.GetOptionLabelLocaleID(nameof(Setting.Cloudiness)), "Current Cloudiness" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.Cloudiness)), $"Use this slider to set the current Cloudiness." },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.Precipitation)), "Current Precipitation (Rain)" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.Precipitation)), $"Use this slider to set the current Precipitation (Rain) volume." },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.Temperature)), "Current Temperature" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.Temperature)), $"Use this slider to change the current temperature. (-50 to +50 degrees)" },

                { _setting.GetOptionLabelLocaleID(nameof(Setting.EnabableTemperature)), "Enable Custom Temperature?" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.EnabableTemperature)), $"Tick to enable a custom temperature value" },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.EnablePrecipitation)), "Enable Custom Precipitation?" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.EnablePrecipitation)), $"Tick to enable a custom precipitation value" },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.EnableCloudiness)), "Enable Custom Cloudiness?" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.EnableCloudiness)), $"Tick to enable a custom cloudiness value" },


                { _setting.GetOptionLabelLocaleID(nameof(Setting.Default)), "Enable Custom Settings" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.Default)), $"Tick to enable custom settings. Leave Unticked for Default." },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.Night)), "Night" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.Night)), $"Sets the time to night" },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.Day)), "Day" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.Day)), $"Sets the time to day" },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.CustomTime)), "Custom Time" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.CustomTime)), $"Slider is between 0 and 23.99 hours." },
                { _setting.GetOptionLabelLocaleID(nameof(Setting.CustomTimeBool)), "Use Custom Time?" },
                { _setting.GetOptionDescLocaleID(nameof(Setting.CustomTimeBool)), $"Enables use of below slider." },

            };
        }

        public void Unload()
        {

        }
    }
}
