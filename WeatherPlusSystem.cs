using System.Threading.Tasks;
using Colossal.Serialization.Entities;
using Game;
using Game.Rendering;
using Game.Simulation;
using UnityEngine;

namespace WeatherPlus;

public enum TimeOfDayOverride
{
    Off = 0,
    Night,
    Day,
    GoldenHour,
}

public partial class WeatherPlusSystem : GameSystemBase
{
    public static WeatherPlusSystem Instance;
    public ClimateSystem _climateSystem;
    public PlanetarySystem _planetarySystem;
    public LightingSystem _lightingSystem;
    public bool IsInitialized;
    private bool seekGoldenHour;
    private float targetVisualTime;

    protected override void OnCreate()
    {
        base.OnCreate();

        Enabled = true;
        Mod.log.Info("OnCreate Ran Successfully");
    }

    protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
    {
        base.OnGameLoadingComplete(purpose, mode);

        if (!mode.IsGameOrEditor())
            return;
        if ( _lightingSystem == null )
        {
            _lightingSystem = World.GetExistingSystemManaged<LightingSystem>( );
            _climateSystem = World.GetExistingSystemManaged<ClimateSystem>( );
            _planetarySystem = World.GetExistingSystemManaged<PlanetarySystem>( );
        }
        UpdateTime();
        UpdateWeather();
        IsInitialized = true;
    }

    public void UpdateTime()
    {
        if (_planetarySystem == null)
            return;
        if ( Mod.m_Setting.FreezeVisualTime && Mod.m_Setting.TimeOfDay == TimeOfDayOverride.Off )
        {
            _planetarySystem.overrideTime = true;
        }
        else
        {
            switch ( Mod.m_Setting.TimeOfDay )
            {
                default:
                case TimeOfDayOverride.Off:
                    if ( !Mod.m_Setting.FreezeVisualTime )
                        _planetarySystem.overrideTime = false;
                    targetVisualTime = 0f;
                    break;

                case TimeOfDayOverride.Night:
                    _planetarySystem.overrideTime = true;
                    targetVisualTime = 0f;
                    break;

                case TimeOfDayOverride.GoldenHour:
                    _planetarySystem.overrideTime = true;

                    _planetarySystem.time = 0f;
                    targetVisualTime = 24f;
                    seekGoldenHour = true;
                    break;

                case TimeOfDayOverride.Day:
                    _planetarySystem.overrideTime = true;
                    targetVisualTime = 12f;
                    break;
            }
        }
    }

    public void UpdateWeather()
    {
        if (_climateSystem == null)
            return;
        _climateSystem.temperature.overrideValue = Mod.m_Setting.Temperature;
        _climateSystem.temperature.overrideState = Mod.m_Setting.EnableTemperature;
        //Mod.log.Warn("Temperature should be at at " + Mod.m_Setting.Temperature);
        //Mod.log.Warn("Temperature is now at " + _climateSystem.temperature.value);

        _climateSystem.precipitation.overrideValue = Mod.m_Setting.Precipitation;
        _climateSystem.precipitation.overrideState = Mod.m_Setting.EnablePrecipitation;

        _climateSystem.cloudiness.overrideValue = Mod.m_Setting.Cloudiness;
        _climateSystem.cloudiness.overrideState = Mod.m_Setting.EnableCloudiness;
    }

    /// <summary>
    /// Check if it's sunrise or sunset
    /// </summary>
    /// <returns></returns>
    private bool IsSunriseOrSunset( )
    {
        return _lightingSystem.state == LightingSystem.State.Sunset || _lightingSystem.state == LightingSystem.State.Sunrise;
    }

    protected override void OnUpdate( )
    {
        if ( !IsInitialized || _planetarySystem == null || !Mod.m_Setting.FreezeVisualTime )
            return;

        if ( seekGoldenHour && IsSunriseOrSunset())
        {
            seekGoldenHour = false;
            Mod.m_Setting.CustomTime = _planetarySystem.time;
        }
        //Mod.log.Info("Current time: " + _planetarySystem.time + "\nTarget time: " + targetVisualTime);
        _planetarySystem.time = Mathf.Lerp( _planetarySystem.time, targetVisualTime , 1.5f * UnityEngine.Time.deltaTime);
        //Mod.log.Info("After Current time: " + _planetarySystem.time + "\nTarget time: " + targetVisualTime);
    }
}