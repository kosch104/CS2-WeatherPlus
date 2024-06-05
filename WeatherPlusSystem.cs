using System.Threading.Tasks;
using Colossal.Serialization.Entities;
using Game;
using Game.Simulation;

namespace WeatherPlus;

public partial class WeatherPlusSystem : GameSystemBase
{
    public static WeatherPlusSystem Instance;
    public ClimateSystem _climateSystem;
    public Mod _mod;
    public PlanetarySystem _planetarySystem;
    public bool isInitialized;
    public float tempCloud;
    public float tempPrecip;
    public float tempTemp;


    public WeatherPlusSystem(Mod mod)
    {
        _mod = mod;
    }

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

            _climateSystem.temperature.overrideState = false;
            _climateSystem.precipitation.overrideState = false;
            _climateSystem.cloudiness.overrideState = false;


            if (_mod.m_Setting.EnableTemperature)
                _climateSystem.temperature.overrideState = true;
            else
                _climateSystem.temperature.overrideState = false;
            if (_mod.m_Setting.EnablePrecipitation)
                _climateSystem.precipitation.overrideState = true;
            else
                _climateSystem.precipitation.overrideState = false;

            if (_mod.m_Setting.EnableCloudiness)
                _climateSystem.cloudiness.overrideState = true;
            else
                _climateSystem.cloudiness.overrideState = false;


            if (_mod == null)
            {
                Mod.log.Warn("Failed to apply settings: _mod is null.");
            }
            else if (_mod.m_Setting == null)
            {
                Mod.log.Warn("Failed to apply settings: _mod.m_Setting is null.");
            }
            else if (_climateSystem != null && _mod.m_Setting != null && isInitialized == false)
            {
                if (_mod.m_Setting.EnableTemperature)
                    _climateSystem.temperature.overrideValue = _mod.m_Setting.Temperature;


                if (_mod.m_Setting.EnablePrecipitation)
                    _climateSystem.precipitation.overrideValue = _mod.m_Setting.Precipitation;

                if (_mod.m_Setting.EnableCloudiness)
                    _climateSystem.cloudiness.overrideValue = _mod.m_Setting.Cloudiness;


                Mod.log.Info("Attempt Apply from GetExistingSystemManaged");
                _mod.m_Setting.Apply();


                isInitialized = true;

                Mod.log.Info("Weather System Initialized");
            }
            else
            {
                Mod.log.Info("Did not run from GetExistingSystemManaged");
            }
        }
        else
        {
            Mod.log.Info("Mode is not game or editor");
            Mod.log.Warn($"Setting is {(_climateSystem == null ? "null" : "not null")}");
        }
    }


    public void UpdateTimeOfDay(bool enableCustomTime, float customTime)
    {
        if (_planetarySystem != null)
        {
            if (enableCustomTime)
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


    public void UpdateWeather(float temperature, float precipitation, float cloudiness)
    {
        Task.Run(async () =>
        {
            await Task.Delay(2000);

            _climateSystem = World.GetExistingSystemManaged<ClimateSystem>();
            Mod.log.Info("UpdateWeather Ran Successfully");


            if (_climateSystem != null)
            {
                _climateSystem.temperature.overrideState = false;
                _climateSystem.precipitation.overrideState = false;
                _climateSystem.cloudiness.overrideState = false;


                if (_mod.m_Setting.EnableTemperature)
                {
                    _climateSystem.temperature.overrideState = true;
                    _climateSystem.temperature.overrideValue = temperature;
                }
                else
                {
                    _climateSystem.temperature.overrideState = false;
                }

                if (_mod.m_Setting.EnablePrecipitation)
                {
                    _climateSystem.precipitation.overrideState = true;
                    _climateSystem.precipitation.overrideValue = precipitation;
                }
                else
                {
                    _climateSystem.precipitation.overrideState = false;
                }

                if (_mod.m_Setting.EnableCloudiness)
                {
                    _climateSystem.cloudiness.overrideState = true;
                    _climateSystem.cloudiness.overrideValue = cloudiness;
                }
                else
                {
                    _climateSystem.cloudiness.overrideState = false;
                }

                Mod.log.Info("Weather updated successfully.");
            }
            else
            {
                Mod.log.Warn("Climate system is null, unable to update weather.");
            }
        });
    }


    protected override void OnUpdate()
    {
    }


    public void OnGameExit()
    {
        isInitialized = false;
    }
}