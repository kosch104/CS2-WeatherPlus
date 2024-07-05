using System.Collections.Generic;
using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;

namespace WeatherPlus;

[FileLocation($"ModsSettings/{nameof(WeatherPlus)}/{nameof(WeatherPlus)}")]
[SettingsUIGroupOrder(kTimeGroup, kDropdownGroup)]
[SettingsUIShowGroupName(kTimeGroup, kDropdownGroup)]
public class Setting : ModSetting
{
    public const string kSectionWeather = "Main";
    public const string kSectionTime = "Time";
    public const string kDropdownGroup = "Dropdown";
    public const string kTimeGroup = "Slider";

    public Setting(IMod mod) : base(mod)
    {
        Mod.log.Info("Setting initialized");
    }

    private TimeOfDayOverride _timeOfDay;

    [SettingsUISection(kSectionTime, kDropdownGroup)]
    public TimeOfDayOverride TimeOfDay
    {
        get => _timeOfDay;
        set
        {
            _timeOfDay = value;
            WeatherPlusSystem.Instance.UpdateTime();
        }
    }

    private bool _freezeVisualTime;

    [SettingsUIHidden]
    [SettingsUISection(kSectionTime, kDropdownGroup)]
    public bool FreezeVisualTime
    {
        get => _freezeVisualTime;
        set
        {
            _freezeVisualTime = value;
            WeatherPlusSystem.Instance.UpdateTime();
        }
    }

    //Page2 - Custom Weather Information

    private bool _enableTemperature;
    [SettingsUISection(kSectionWeather, kTimeGroup)]
    public bool EnableTemperature
    {
        get => _enableTemperature;
        set
        {
            _enableTemperature = value;
            WeatherPlusSystem.Instance.UpdateWeather();
        }
    }

    private float _temperature;
    [SettingsUISlider(min = -50, max = 50, step = 1, scalarMultiplier = 1, unit = Unit.kTemperature)]
    [SettingsUISection(kSectionWeather, kTimeGroup)]
    public float Temperature
    {
        get => _temperature;
        set
        {
            _temperature = value;
            EnableTemperature = true;
            WeatherPlusSystem.Instance.UpdateWeather();
        }
    }

    private bool _enablePrecipitation;
    [SettingsUISection(kSectionWeather, kTimeGroup)]
    public bool EnablePrecipitation
    {
        get => _enablePrecipitation;
        set
        {
            _enablePrecipitation = value;
            WeatherPlusSystem.Instance.UpdateWeather();
        }
    }

    private float _precipitation;
    [SettingsUISlider(min = 0.000f, max = 0.999f, step = 0.001f, scalarMultiplier = 1,
        unit = Unit.kFloatThreeFractions)]
    [SettingsUISection(kSectionWeather, kTimeGroup)]
    public float Precipitation
    {
        get => _precipitation;
        set
        {
            _precipitation = value;
            _enablePrecipitation = true;
            WeatherPlusSystem.Instance.UpdateWeather();
        }
    }

    private bool _enableCloudiness;
    [SettingsUISection(kSectionWeather, kTimeGroup)]
    public bool EnableCloudiness
    {
        get => _enableCloudiness;
        set
        {
            _enableCloudiness = value;
            WeatherPlusSystem.Instance.UpdateWeather();
        }
    }

    private float _cloudiness;
    [SettingsUISlider(min = 0.000f, max = 0.999f, step = 0.001f, scalarMultiplier = 1,
        unit = Unit.kFloatThreeFractions)]
    [SettingsUISection(kSectionWeather, kTimeGroup)]
    public float Cloudiness
    {
        get => _cloudiness;
        set
        {
            _cloudiness = value;
            EnableTemperature = true;
            WeatherPlusSystem.Instance.UpdateWeather();
        }
    }


    //Page3 - Time Information

    private float _customTime;

    [SettingsUISlider(min = 0, max = 23.99f, step = 0.10f, scalarMultiplier = 1, unit = Unit.kFloatTwoFractions)]
    [SettingsUISection(kSectionTime, kDropdownGroup)]
    [SettingsUIHidden]
    public float CustomTime
    {
        get => _customTime;
        set
        {
            _customTime = value;
            //FreezeVisualTime = true;
        }
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
            { _setting.GetOptionGroupLocaleID(Setting.kTimeGroup), "Change Current Weather" },
            { _setting.GetOptionDescLocaleID(Setting.kSectionWeather), "Change the current weather settings." },
            {
                _setting.GetOptionGroupLocaleID(Setting.kDropdownGroup),
                "Choose a Custom Time"
            },


            { _setting.GetOptionLabelLocaleID(nameof(Setting.TimeOfDay)), "Time of Day Override" },
            {
                _setting.GetOptionDescLocaleID(nameof(Setting.TimeOfDay)),
                "Set a preset to override the time of day. Set to off to disable."
            },

            { _setting.GetEnumValueLocaleID(TimeOfDayOverride.Off), "Off" },
            { _setting.GetEnumValueLocaleID(TimeOfDayOverride.Day), "Day" },
            { _setting.GetEnumValueLocaleID(TimeOfDayOverride.Night), "Night" },
            { _setting.GetEnumValueLocaleID(TimeOfDayOverride.GoldenHour), "Golden Hour (WIP)" },


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

            { _setting.GetOptionLabelLocaleID(nameof(Setting.CustomTime)), "Custom Time" },
            { _setting.GetOptionDescLocaleID(nameof(Setting.CustomTime)), "Slider is between 0 and 23.99 hours." },
            { _setting.GetOptionLabelLocaleID(nameof(Setting.FreezeVisualTime)), "Freeze Visual Time" },
            { _setting.GetOptionDescLocaleID(nameof(Setting.FreezeVisualTime)), "Enables use of below slider." }
        };
    }

    public void Unload()
    {
    }
}