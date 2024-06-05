using System.Collections.Generic;
using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;

namespace WeatherPlus;

[FileLocation(nameof(WeatherPlus))]
[SettingsUIGroupOrder(kButtonGroupPresets, kSliderGroup, kDropdownGroup)]
[SettingsUIShowGroupName(kButtonGroupPresets, kSliderGroup, kDropdownGroup)]
public class Setting : ModSetting
{
    public const string kSectionWeather = "Main";
    public const string kSectionTime = "Time";
    public const string kSectionPresets = "Presets";
    public const string kButtonGroupPresets = "Button2";
    public const string kDropdownGroup = "Dropdown";
    public const string kSliderGroup = "Slider";

    public Setting(IMod mod) : base(mod)
    {
        Mod.log.Info("Setting initialized");


        WeatherPlusSystem.Instance.UpdateWeather(Temperature, Precipitation, Cloudiness);
        WeatherPlusSystem.Instance.UpdateTimeOfDay(EnableCustomTime, CustomTime);
    }

    //Page1 - Presets

    [SettingsUIHidden]
    public bool HiddenSetting { get; set; }

    [SettingsUISection(kSectionPresets, kButtonGroupPresets)]
    public bool TimeSixAM
    {
        set => CustomTime = 6f;
    }

    [SettingsUISection(kSectionPresets, kButtonGroupPresets)]
    public bool TimeSevenAM
    {
        set => CustomTime = 7f;
    }

    [SettingsUISection(kSectionPresets, kButtonGroupPresets)]
    public bool Day
    {
        set => CustomTime = 13f;
    }

    [SettingsUISection(kSectionPresets, kButtonGroupPresets)]
    public bool Night
    {
        set => CustomTime = 22f;
    }


    //Page2 - Custom Weather Information

    [SettingsUISection(kSectionWeather, kSliderGroup)]
    public bool EnableTemperature { get; set; }

    [SettingsUISlider(min = -50, max = 50, step = 1, scalarMultiplier = 1, unit = Unit.kTemperature)]
    [SettingsUISection(kSectionWeather, kSliderGroup)]
    public float Temperature { get; set; }

    [SettingsUISection(kSectionWeather, kSliderGroup)]
    public bool EnablePrecipitation { get; set; }

    [SettingsUISlider(min = 0.000f, max = 0.999f, step = 0.001f, scalarMultiplier = 1,
        unit = Unit.kFloatThreeFractions)]
    [SettingsUISection(kSectionWeather, kSliderGroup)]
    public float Precipitation { get; set; }

    [SettingsUISection(kSectionWeather, kSliderGroup)]
    public bool EnableCloudiness { get; set; }

    [SettingsUISlider(min = 0.000f, max = 0.999f, step = 0.001f, scalarMultiplier = 1,
        unit = Unit.kFloatThreeFractions)]
    [SettingsUISection(kSectionWeather, kSliderGroup)]
    public float Cloudiness { get; set; }


    //Page3 - Time Information


    [SettingsUISection(kSectionTime, kDropdownGroup)]
    public bool EnableCustomTime { get; set; }


    [SettingsUISlider(min = 0, max = 23.99f, step = 0.10f, scalarMultiplier = 1, unit = Unit.kFloatTwoFractions)]
    [SettingsUISection(kSectionTime, kDropdownGroup)]
    public float CustomTime { get; set; }


    public override void Apply()
    {
        Mod.log.Info("Running Apply method...");

        WeatherPlusSystem.Instance.UpdateWeather(Temperature, Precipitation, Cloudiness);
        WeatherPlusSystem.Instance.UpdateTimeOfDay(EnableCustomTime, CustomTime);

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

    public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors,
        Dictionary<string, int> indexCounts)
    {
        return new Dictionary<string, string>
        {
            { _setting.GetSettingsLocaleID(), "WeatherPlus" },
            { _setting.GetOptionTabLocaleID(Setting.kSectionWeather), "Weather Settings" },
            { _setting.GetOptionTabLocaleID(Setting.kSectionTime), "Time Settings" },
            { _setting.GetOptionTabLocaleID(Setting.kSectionPresets), "Presets" },
            { _setting.GetOptionGroupLocaleID(Setting.kButtonGroupPresets), "Presets" },
            { _setting.GetOptionGroupLocaleID(Setting.kSliderGroup), "Change Current Weather" },
            { _setting.GetOptionDescLocaleID(Setting.kSectionWeather), "Change the current weather settings." },
            {
                _setting.GetOptionGroupLocaleID(Setting.kDropdownGroup),
                "Choose a Custom Time"
            },


            { _setting.GetOptionLabelLocaleID(nameof(Setting.TimeSixAM)), "6:00AM" },
            { _setting.GetOptionDescLocaleID(nameof(Setting.TimeSixAM)), "Sets the time to 6:00AM" },
            { _setting.GetOptionLabelLocaleID(nameof(Setting.TimeSevenAM)), "7:00AM" },
            { _setting.GetOptionDescLocaleID(nameof(Setting.TimeSevenAM)), "Sets the time to 7:00AM" },


            { _setting.GetOptionLabelLocaleID(nameof(Setting.Cloudiness)), "Current Cloudiness" },
            {
                _setting.GetOptionDescLocaleID(nameof(Setting.Cloudiness)),
                "Use this slider to set the current Cloudiness."
            },
            { _setting.GetOptionLabelLocaleID(nameof(Setting.Precipitation)), "Current Precipitation (Rain)" },
            {
                _setting.GetOptionDescLocaleID(nameof(Setting.Precipitation)),
                "Use this slider to set the current Precipitation (Rain) volume."
            },
            { _setting.GetOptionLabelLocaleID(nameof(Setting.Temperature)), "Current Temperature" },
            {
                _setting.GetOptionDescLocaleID(nameof(Setting.Temperature)),
                "Use this slider to change the current temperature. (-50 to +50 degrees)"
            },

            { _setting.GetOptionLabelLocaleID(nameof(Setting.EnableTemperature)), "Enable Custom Temperature?" },
            {
                _setting.GetOptionDescLocaleID(nameof(Setting.EnableTemperature)),
                "Tick to enable a custom temperature value"
            },
            { _setting.GetOptionLabelLocaleID(nameof(Setting.EnablePrecipitation)), "Enable Custom Precipitation?" },
            {
                _setting.GetOptionDescLocaleID(nameof(Setting.EnablePrecipitation)),
                "Tick to enable a custom precipitation value"
            },
            { _setting.GetOptionLabelLocaleID(nameof(Setting.EnableCloudiness)), "Enable Custom Cloudiness?" },
            {
                _setting.GetOptionDescLocaleID(nameof(Setting.EnableCloudiness)),
                "Tick to enable a custom cloudiness value"
            },

            { _setting.GetOptionLabelLocaleID(nameof(Setting.Night)), "Night" },
            { _setting.GetOptionDescLocaleID(nameof(Setting.Night)), "Sets the time to night" },
            { _setting.GetOptionLabelLocaleID(nameof(Setting.Day)), "Day" },
            { _setting.GetOptionDescLocaleID(nameof(Setting.Day)), "Sets the time to day" },
            { _setting.GetOptionLabelLocaleID(nameof(Setting.CustomTime)), "Custom Time" },
            { _setting.GetOptionDescLocaleID(nameof(Setting.CustomTime)), "Slider is between 0 and 23.99 hours." },
            { _setting.GetOptionLabelLocaleID(nameof(Setting.EnableCustomTime)), "Use Custom Time?" },
            { _setting.GetOptionDescLocaleID(nameof(Setting.EnableCustomTime)), "Enables use of below slider." }
        };
    }

    public void Unload()
    {
    }
}