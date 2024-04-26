using Game.Tools;
using Gooee.Plugins.Attributes;
using Gooee.Plugins;
using Unity.Entities;
using System;
using WeatherPlus.Models;
using System.Data;
using Game.Simulation;
using Microsoft.CSharp;
using Colossal.Logging;
using Gooee;
using Gooee.Helpers;
using Unity.Mathematics;
using Game.Prefabs.Climate;
using UnityEngine;
using static Game.Prefabs.Climate.ClimatePrefab;
using System.Reflection;
using Colossal.UI.Binding;
using Game.SceneFlow;
using System.Linq;

namespace WeatherPlus.UI
{
    public partial class WeatherPlusController : Controller<WeatherPlusModel>
    {
        public DanielsWeatherSystem _weatherSystem;
        public int IntRain = 0;
        public float FloatRain = 0f;
        public int IntClouds = 0;
        public float FloatClouds = 0f;


        public override WeatherPlusModel Configure()
        {
            _weatherSystem = World.GetOrCreateSystemManaged<DanielsWeatherSystem>( );


            Mod.DebugLog("Weather System found on Controller");



            



            return new WeatherPlusModel( );
        }

        public void UpdateSettings()
        {
            Mod.m_Setting.OverrideTime = Model.TimeOverride;
            Mod.m_Setting.Time = Model.Time;
            Mod.m_Setting.CloudAmount = Model.CloudsAmount;
            Mod.m_Setting.CloudsOverride = Model.CloudsOverride;
            Mod.m_Setting.RainOverride = Model.RainOverride;
            Mod.m_Setting.RainAmount = Model.RainAmount;
            Mod.m_Setting.Temperature = Model.Temperature;
            Mod.m_Setting.TemperatureOverride = Model.TemperatureOverride;
        }


        public void UpdateModelRain()
        {
            float temperature = _weatherSystem._climateSystem.temperature.overrideValue;
            float cloudiness = _weatherSystem._climateSystem.cloudiness.overrideValue;
            float precipitation = _weatherSystem._climateSystem.precipitation.overrideValue;

            string temperatureMessage = "";
            string precipitationMessage = "";
            string cloudinessMessage = "";

            // Determine temperature message
            if (temperature < 0)
            {
                temperatureMessage = "Freezing";
            }
            else if (temperature > 30)
            {
                temperatureMessage = "Hot";
            }
            else if (temperature > 10)
            {
                temperatureMessage = "Warm";
            }
            else
            {
                temperatureMessage = "Chilly";
            }

            // Determine precipitation message
            if (precipitation == 0)
            {
                precipitationMessage = "no precipitation";
            }
            else if (precipitation <= 0.500)
            {
                precipitationMessage = "raining";
            }
            else
            {
                precipitationMessage = "heavy rain";
            }

            // Determine cloudiness message
            if (cloudiness == 0)
            {
                cloudinessMessage = "clear sky";
            }
            else if (cloudiness <= 0.500)
            {
                cloudinessMessage = "cloudy";
            }
            else
            {
                cloudinessMessage = "heavy clouds";
            }

            // Combine messages
            Model.MessageAdvanced = $"{temperatureMessage}, {precipitationMessage}, and {cloudinessMessage}.";
            
        }



        [OnTrigger]
        private void OnOverrideTemperature()
        {
            Mod.DebugLog("OnOverrideTemperature Ran Correctly.");

            if (_weatherSystem._climateSystem != null && Model.TemperatureOverride == true)
            {
                _weatherSystem._climateSystem.temperature.overrideState = true;
                Mod.m_Setting.TemperatureOverride = true;
                Mod.DebugLog("Temperature override state set successfully" + Model.TemperatureOverride);
            } 
            else if (_weatherSystem._climateSystem != null && Model.TemperatureOverride == false)
                {
                _weatherSystem._climateSystem.temperature.overrideState = false;
                Mod.m_Setting.TemperatureOverride = false;
                Mod.DebugLog("Temperature override state set successfully to false" + Model.TemperatureOverride);
                if (Model.RainOverride == false && Model.CloudsOverride == false)
                {
                    Model.MessageAdvanced = "Using Default Settings";
                    Model.MessageRain = "Using Default Settings";
                }
            }

            else
            {
                Mod.DebugLog("Planetary or Weather System is null IN TEMPREATURE");
            }


            Update();
            TriggerUpdate();
            UpdateSettings();
        }

        [OnTrigger]
        private void OnSetTemperature()
        {
            Mod.DebugLog("OnSetTemperature Ran Correctly.");

            

            if (_weatherSystem._climateSystem != null)
            {
                _weatherSystem._climateSystem.temperature.overrideValue = Model.Temperature;
                Mod.m_Setting.Temperature = Model.Temperature;
                Model.TemperatureOverride = true;
                Mod.DebugLog("Temperature successfully set " + Model.Temperature);
            }
            else
            {
                Mod.DebugLog("Planetary or Weather System is null IN TEMPREATURE");
            }



            UpdateModelRain();
            Update();
            TriggerUpdate();
            UpdateSettings();
        }

        [OnTrigger]
        private void OnRainOverride()
        {
            Mod.DebugLog("OnRainOverride Ran Correctly.");

            if (_weatherSystem._climateSystem != null && Model.RainOverride == true)
            {
                _weatherSystem._climateSystem.precipitation.overrideState = true;
                Mod.m_Setting.RainOverride = true;
                Mod.DebugLog("Rain override state set successfully" + Model.RainOverride);
            }
            else if (_weatherSystem._climateSystem != null && Model.RainOverride == false)
            {
                _weatherSystem._climateSystem.precipitation.overrideState = false;
                Mod.m_Setting.RainOverride = false;
                Mod.DebugLog("Rain override state set successfully to false" + Model.RainOverride);
                if (Model.TemperatureOverride == false && Model.CloudsOverride == false)
                {
                    Model.MessageAdvanced = "Using Default Settings";
                    Model.MessageRain = "Using Default Settings";
                }
            }

            else
            {
                Mod.DebugLog("Planetary or Weather System is null IN Percipitation");
            }


            Update();
            TriggerUpdate();
            UpdateSettings();
        }

        [OnTrigger]
        private void onRainChanged()
        {
            Mod.DebugLog("onRainChanged Triggered: Initial RainAmount value: " + Model.RainAmount); // Log before 




            if (_weatherSystem._climateSystem != null)
            {
                IntRain = Model.RainAmount;
                FloatRain = IntRain / 100f;
                Mod.DebugLog("Converted Rain: " + FloatRain);
                _weatherSystem._climateSystem.precipitation.overrideValue = FloatRain;
                Mod.m_Setting.RainAmount = FloatRain;
                Model.RainOverride = true;
                Mod.m_Setting.RainOverride = true;
                Mod.DebugLog("Rain successfully set: Updated RainAmount value: " + Model.RainAmount); // Log after
            }
            else
            {
                Mod.DebugLog("Planetary or Weather System is null IN Precipitation");
            }
            UpdateModelRain();
            Update();
            TriggerUpdate();
            UpdateSettings();
        }

        [OnTrigger]
        private void OnCloudsOverride()
        {
            Mod.DebugLog("OnCloudsOverride Ran Correctly.");

            if (_weatherSystem._climateSystem != null && Model.CloudsOverride == true)
            {
                _weatherSystem._climateSystem.cloudiness.overrideState = true;
                Mod.m_Setting.CloudsOverride = true;
                Mod.DebugLog("Clouds override state set successfully" + Model.CloudsOverride);
            }
            else if (_weatherSystem._climateSystem != null && Model.CloudsOverride == false)
            {
                _weatherSystem._climateSystem.cloudiness.overrideState = false;
                Mod.m_Setting.CloudsOverride = false;
                Mod.DebugLog("Clouds override state set successfully to false" + Model.CloudsOverride);

                if (Model.RainOverride == false && Model.TemperatureOverride == false)
                {
                    Model.MessageAdvanced = "Using Default Settings";
                    Model.MessageRain = "Using Default Settings";
                }
            }

            else
            {
                Mod.DebugLog("Planetary or Weather System is null IN Percipitation");
            }


            Update();
            TriggerUpdate();
            UpdateSettings();
        }

        [OnTrigger]
        private void onCloudsChanged()
        {
            Mod.DebugLog("onCloudsChanged Triggered: Initial CloudsAmount value: " + Model.CloudsAmount); // Log before 


            if (_weatherSystem._climateSystem != null)
            {
                IntClouds = Model.CloudsAmount;
                FloatClouds = IntClouds / 100f;
                Mod.DebugLog("Converted Clouds: " + FloatClouds);
                _weatherSystem._climateSystem.cloudiness.overrideValue = FloatClouds;
                Mod.m_Setting.CloudAmount = FloatClouds;
                Model.CloudsOverride = true;
                OnCloudsOverride();
                Mod.DebugLog("Clouds successfully set: Updated RainAmount value: " + Model.CloudsAmount); // Log after
            }
            else
            {
                Mod.DebugLog("Planetary or Weather System is null IN Precipitation");
            }

            UpdateModelRain();
            Update();
            TriggerUpdate();
            UpdateSettings();
        }

        [OnTrigger]
        private void OnSetRain()
        {
            if (Model != null)
            {
                Model.MessageRain = "Currently Raining";
            }
            else
            {
                Mod.DebugLog("Model is null");
                return; 
            }

            if (_weatherSystem._climateSystem != null)
            {
                _weatherSystem._climateSystem.precipitation.overrideState = true;
                _weatherSystem._climateSystem.precipitation.overrideValue = 0.998f;
                Mod.m_Setting.RainAmount = (int)0.999;
                Mod.m_Setting.RainOverride = true;
                Model.RainOverride = true;
                Model.RainAmount = (int)0.999;

                _weatherSystem._climateSystem.cloudiness.overrideState = true;
                _weatherSystem._climateSystem.cloudiness.overrideValue = 0.998f;
                Mod.m_Setting.CloudAmount = 0.999f;
                Mod.m_Setting.CloudsOverride = true;
                Model.CloudsOverride = true;
                Model.CloudsAmount = (int)0.999;
                _weatherSystem._climateSystem.temperature.overrideState = true;
                _weatherSystem._climateSystem.temperature.overrideValue = 10.0f;
                Mod.m_Setting.Temperature = 10;
                Mod.m_Setting.TemperatureOverride = true;
                Mod.DebugLog("Rain successfully set");
            }
            else
            {
                Mod.DebugLog("Planetary or Weather System is null IN RAIN");
            }

            TriggerUpdate();
            UpdateSettings();
        }

        [OnTrigger]
        
        private void OnSetSnow()
        {
            if (Model != null)
            {
                Model.MessageRain = "Currently Snowing";
            }
            else
            {

                Mod.DebugLog("Model is null");
                return; 
            }


            if (_weatherSystem._climateSystem != null)
            {
                
                _weatherSystem._climateSystem.precipitation.overrideState = true;
                _weatherSystem._climateSystem.precipitation.overrideValue = 0.998f;
                Mod.m_Setting.RainAmount = (int)0.999;
                Mod.m_Setting.RainOverride = true;
                Model.RainAmount = (int)0.999;
                Model.RainOverride = true;
                _weatherSystem._climateSystem.cloudiness.overrideState = true;
                _weatherSystem._climateSystem.cloudiness.overrideValue = 0.998f;
                Mod.m_Setting.CloudAmount = 0.999f;
                Mod.m_Setting.CloudsOverride = true;
                Model.CloudsAmount = (int)0.999;
                Model.CloudsOverride = true;
                _weatherSystem._climateSystem.temperature.overrideState = true;
                _weatherSystem._climateSystem.temperature.overrideValue = -10.0f;
                Mod.m_Setting.Temperature = -10;
                Mod.m_Setting.TemperatureOverride = true;
                Mod.DebugLog("Snow successfully set");
            }
            else
            {
                Mod.DebugLog("Planetary or Weather System is null IN SNOW");
            }

            TriggerUpdate();
            UpdateSettings();
        }

      

        [OnTrigger]
        private void OnSetSun()
        {
            if (Model != null)
            {
                Model.MessageRain = "Currently Sunny";
            }
            else
            {

                Mod.DebugLog("Model is null");
                return;
            }


            if (_weatherSystem._climateSystem != null)
            {

                _weatherSystem._climateSystem.precipitation.overrideState = true;
 
                Mod.m_Setting.RainOverride = true;
                Model.RainOverride = true;
                _weatherSystem._climateSystem.cloudiness.overrideState = true;
                Mod.m_Setting.CloudsOverride = true;
                Model.CloudsOverride = true;
                _weatherSystem._climateSystem.cloudiness.overrideValue = 0.0f;
                Mod.m_Setting.CloudAmount = 0.0f;
                Model.CloudsAmount = 0;
                _weatherSystem._climateSystem.precipitation.overrideValue = 0.0f;
                Mod.m_Setting.RainAmount = (int)0.0;
                Model.RainAmount = 0;
                _weatherSystem._climateSystem.temperature.overrideState = true;
                Mod.m_Setting.TemperatureOverride = true;
                _weatherSystem._climateSystem.temperature.overrideValue = 26.0f;
                Mod.m_Setting.Temperature = 26;

                Mod.DebugLog("Sun successfully set");
            }
            else
            {
                Mod.DebugLog("Planetary or Weather System is null");
            }

            TriggerUpdate();
            UpdateSettings();
        }

        [OnTrigger]
        private void OnSetDefaults()
        {
            if (Model != null)
            {
                Model.MessageRain = "Using Default Settings";
            }
            else
            {
                Mod.DebugLog("Model is null");
                return;
            }


            if (_weatherSystem._climateSystem != null)
            {
                _weatherSystem._climateSystem.precipitation.overrideState = false;
                Mod.m_Setting.RainOverride = false;
                Model.RainOverride = false;
                _weatherSystem._climateSystem.cloudiness.overrideState = false;
                Mod.m_Setting.CloudsOverride = false;
                _weatherSystem._climateSystem.temperature.overrideState = false;
                Mod.m_Setting.TemperatureOverride = false;
                Model.TemperatureOverride = false;
                Mod.DebugLog("Night successfully set");
                Model.Temperature = (int)_weatherSystem._climateSystem.temperature.value;



                Mod.DebugLog("Sun successfully set");
            }
            else
            {

                Mod.DebugLog("Planetary or Weather System is null");
            }

            TriggerUpdate();
            UpdateSettings();
        }

        [OnTrigger]
        private void OnSetNight()
        {
            if (Model != null)
            {
                Model.Message = "Night Time";
            }
            else
            {

                Mod.DebugLog("Model is null");
                return; 
            }

            if (_weatherSystem != null && _weatherSystem._planetarySystem != null)
            {
                _weatherSystem._planetarySystem.overrideTime = true;
                _weatherSystem._planetarySystem.time = 0f;
                Mod.m_Setting.OverrideTime = true;
                Mod.m_Setting.Time = 0f;
                Mod.DebugLog("Night successfully set");
            }
            else
            {
                Mod.DebugLog("Planetary or Weather System is null");
            }

            TriggerUpdate();
            UpdateSettings();
        }

        [OnTrigger]
        private void OnSetDefault()
        {
            if (Model != null)
            {
                Model.Message = "Using Default Settings";
            }
            else
            {

                Mod.DebugLog("Model is null");
                return;
            }

            if (_weatherSystem != null && _weatherSystem._planetarySystem != null)
            {
                _weatherSystem._planetarySystem.overrideTime = false;
                Mod.m_Setting.OverrideTime = false;
                Mod.DebugLog("Night successfully set");
            }
            else
            {
                Mod.DebugLog("Planetary or Weather System is null");
            }

            TriggerUpdate();
            UpdateSettings();
        }

        [OnTrigger]
        private void OnSetDay()
        {
            if (Model != null)
            {
                Model.Message = "Day Time ";
            }
            else
            {

                Mod.DebugLog("Model is null");
                return; 
            }

            if (_weatherSystem != null && _weatherSystem._planetarySystem != null)
            {
                _weatherSystem._planetarySystem.overrideTime = true;
                _weatherSystem._planetarySystem.time = 12f;
                Model.TimeOverride = true;
                Mod.m_Setting.OverrideTime = true;
                Mod.m_Setting.Time = 12f;
                Mod.DebugLog("Night successfully set");
            }
            else
            {
                Mod.DebugLog("Planetary or Weather System is null");
            }

            TriggerUpdate();
            UpdateSettings();
        }

        [OnTrigger]
        private void OnSetTime()
        {
            if (Model != null)
            {
                Model.Message = "Custom Time";
            }
            else
            {

                Mod.DebugLog("Model is null");
                return;
            }
            float oldvalue = Model.Time;
            float newvalue = ConvertToNewRange(oldvalue, 0, 24);
            if (_weatherSystem != null && _weatherSystem._planetarySystem != null)
            {
                _weatherSystem._planetarySystem.overrideTime = true;
                _weatherSystem._planetarySystem.time = newvalue;
                Mod.m_Setting.OverrideTime = true;
                Mod.m_Setting.Time = Model.Time;
                Mod.DebugLog("Custom Time Set Successfully");
            }
            else
            {
                Mod.DebugLog("Planetary or Weather System is null");
            }

            TriggerUpdate();
            UpdateSettings();
        }

        [OnTrigger]
        private void OnToggleVisible( )
        {
            Model.IsVisible = true;
            TriggerUpdate( );
        }

        public float ConvertToNewRange(float value, float newMin, float newMax)
        {
            // Calculate the proportionate value in the new range
            float newValue = (value / 100f) * (newMax - newMin) + newMin;

            return newValue;
        }
    }
}
