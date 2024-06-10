using System.Threading.Tasks;
using Colossal.Serialization.Entities;
using Game;
using Game.Simulation;

namespace WeatherPlus;

public partial class WeatherPlusSystem : GameSystemBase
{
    public static WeatherPlusSystem Instance;
    public ClimateSystem _climateSystem;
    public PlanetarySystem _planetarySystem;

    protected override void OnCreate()
    {
        base.OnCreate();


        Mod.log.Info("OnCreate Ran Successfully");
        _climateSystem = World.GetExistingSystemManaged<ClimateSystem>();
    }

    protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
    {
        base.OnGameLoadingComplete(purpose, mode);

        if (!mode.IsGameOrEditor())

            return;


        if (mode.IsGameOrEditor())
        {
            _climateSystem = World.GetExistingSystemManaged<ClimateSystem>();
            Mod.log.Info("Climate System found");

            _planetarySystem = World.GetExistingSystemManaged<PlanetarySystem>();
            Mod.log.Info("Climate System found");
            UpdateTime();
            UpdateWeather();
        }
        else
        {
            Mod.log.Info("Mode is not game or editor");
            Mod.log.Warn($"Setting is {(_climateSystem == null ? "null" : "not null")}");
        }
    }

    public void UpdateTime()
    {
        UpdateTime(Mod.m_Setting.EnableCustomTime, Mod.m_Setting.CustomTime);
    }

    private void UpdateTime(bool overrideTime, float customTime)
    {
        if (_planetarySystem != null)
        {
            if (overrideTime)
            {
                _planetarySystem.overrideTime = true;
                _planetarySystem.time = customTime;
            }

            else
            {
                _planetarySystem.overrideTime = false;

            }
        }
        else if (_planetarySystem != null)
        {
            _planetarySystem.overrideTime = false;
        }
        else
        {
            Mod.log.Warn("Planetary system is null, unable to update time of day.");
        }
    }

    public void UpdateWeather()
    {
        UpdateWeather(Mod.m_Setting.EnableTemperature, Mod.m_Setting.Temperature, Mod.m_Setting.EnablePrecipitation, Mod.m_Setting.Precipitation, Mod.m_Setting.EnableCloudiness, Mod.m_Setting.Cloudiness);
    }

    private void UpdateWeather(bool overrideTemperature, float temperature, bool overridePrecipitation, float precipitation, bool overrideCloudiness, float cloudiness)
    {
        _climateSystem = World.GetExistingSystemManaged<ClimateSystem>();

        if (_climateSystem != null)
        {
            _climateSystem.temperature.overrideState = overrideTemperature;
            _climateSystem.temperature.value = temperature;

            _climateSystem.precipitation.overrideState = overridePrecipitation;
            _climateSystem.precipitation.value = precipitation;

            _climateSystem.cloudiness.overrideState = overrideCloudiness;
            _climateSystem.cloudiness.value = cloudiness;


            Mod.log.Info("Weather updated successfully.");
        }
        else
        {
            Mod.log.Warn("Climate system is null, unable to update weather.");
        }
        /*Task.Run(async () =>
        {
            await Task.Delay(2000);

            _climateSystem = World.GetExistingSystemManaged<ClimateSystem>();

            if (_climateSystem != null)
            {
                _climateSystem.temperature.overrideState = overrideTemperature;
                _climateSystem.temperature.value = temperature;

                _climateSystem.precipitation.overrideState = overridePrecipitation;
                _climateSystem.precipitation.value = precipitation;

                _climateSystem.cloudiness.overrideState = overrideCloudiness;
                _climateSystem.cloudiness.value = cloudiness;


                Mod.log.Info("Weather updated successfully.");
            }
            else
            {
                Mod.log.Warn("Climate system is null, unable to update weather.");
            }
        });*/
    }


    protected override void OnUpdate()
    {
    }
}