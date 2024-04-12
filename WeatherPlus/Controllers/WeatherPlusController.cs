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

namespace WeatherPlus.UI
{
    public partial class WeatherPlusController : Controller<WeatherPlusModel>
    {
        public DanielsWeatherSystem _weatherSystem;


        public float newValue = 48;


        public override WeatherPlusModel Configure( )
        {
            _weatherSystem = World.GetOrCreateSystemManaged<DanielsWeatherSystem>( );
            Mod.DebugLog("Weather System found on Controller");


            return new WeatherPlusModel( );
        }



        [OnTrigger]
        private void OnOverrideTemperature()
        {
            Mod.DebugLog("OnOverrideTemperature Ran Correctly.");

            if (_weatherSystem._climateSystem != null && Model.TemperatureOverride == true)
            {
                _weatherSystem._climateSystem.temperature.overrideState = true;
                Mod.DebugLog("Temperature override state set successfully" + Model.TemperatureOverride);
            } 
            else if (_weatherSystem._climateSystem != null && Model.TemperatureOverride == false)
                {
                _weatherSystem._climateSystem.temperature.overrideState = false;
                Mod.DebugLog("Temperature override state set successfully to false" + Model.TemperatureOverride);
            }

            else
            {
                Mod.DebugLog("Planetary or Weather System is null IN TEMPREATURE");
            }

            Update();
        }

        [OnTrigger]
        private void OnSetTemperature()
        {
            Mod.DebugLog("OnSetTemperature Ran Correctly.");

            

            if (_weatherSystem._climateSystem != null)
            {
                _weatherSystem._climateSystem.temperature.overrideValue = Model.Temperature;
                Model.TemperatureOverride = true;
                Mod.DebugLog("Temperature successfully set " + Model.Temperature);
            }
            else
            {
                // Handle the case where _weatherSystem or _planetarySystem is null
                Mod.DebugLog("Planetary or Weather System is null IN TEMPREATURE");
            }
            Update();
            TriggerUpdate();
        }

        [OnTrigger]
        private void OnSetRain()
        {
            // Amend the message in the model
            if (Model != null)
            {
                Model.MessageRain = "Currently Raining";
            }
            else
            {
                // Handle the case where Model is null
                // Log or handle the error accordingly
                Mod.DebugLog("Model is null");
                return; // Exit the method early
            }

            // Check if _weatherSystem and _planetarySystem are not null before accessing their members
            if (_weatherSystem._climateSystem != null)
            {
                // Update the time in _planetarySystem
                _weatherSystem._climateSystem.precipitation.overrideState = true;
                _weatherSystem._climateSystem.precipitation.overrideValue = 0.998f;
                _weatherSystem._climateSystem.cloudiness.overrideState = true;
                _weatherSystem._climateSystem.cloudiness.overrideValue = 0.998f;
                _weatherSystem._climateSystem.temperature.overrideState = true;
                _weatherSystem._climateSystem.temperature.overrideValue = 10.0f;
                Mod.DebugLog("Rain successfully set");
            }
            else
            {
                // Handle the case where _weatherSystem or _planetarySystem is null
                Mod.DebugLog("Planetary or Weather System is null IN RAIN");
            }

            // Trigger an update (assuming TriggerUpdate() is a valid method)
            TriggerUpdate();
        }

        [OnTrigger]
        
        private void OnSetSnow()
        {
            // Amend the message in the model
            if (Model != null)
            {
                Model.MessageRain = "Currently Snowing";
            }
            else
            {
                // Handle the case where Model is null
                // Log or handle the error accordingly
                Mod.DebugLog("Model is null");
                return; // Exit the method early
            }

            // Check if _weatherSystem and _planetarySystem are not null before accessing their members
            if (_weatherSystem._climateSystem != null)
            {
                // Update the time in _planetarySystem
                _weatherSystem._climateSystem.precipitation.overrideState = true;
                _weatherSystem._climateSystem.precipitation.overrideValue = 0.998f;
                _weatherSystem._climateSystem.cloudiness.overrideState = true;
                _weatherSystem._climateSystem.cloudiness.overrideValue = 0.998f;
                _weatherSystem._climateSystem.temperature.overrideState = true;
                _weatherSystem._climateSystem.temperature.overrideValue = -10.0f;
                Mod.DebugLog("Snow successfully set");
            }
            else
            {
                Mod.DebugLog("Planetary or Weather System is null IN SNOW");
            }

            TriggerUpdate();
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
                return; // Exit the method early
            }


            if (_weatherSystem._climateSystem != null)
            {

                _weatherSystem._climateSystem.precipitation.overrideState = true;
                _weatherSystem._climateSystem.cloudiness.overrideState = true;
                _weatherSystem._climateSystem.cloudiness.overrideValue = 0.0f;
                _weatherSystem._climateSystem.precipitation.overrideValue = 0.0f;
                _weatherSystem._climateSystem.temperature.overrideState = true;
                _weatherSystem._climateSystem.temperature.overrideValue = 26.0f;

                Mod.DebugLog("Sun successfully set");
            }
            else
            {
                // Handle the case where _weatherSystem or _planetarySystem is null
                Mod.DebugLog("Planetary or Weather System is null");
            }

            // Trigger an update (assuming TriggerUpdate() is a valid method)
            TriggerUpdate();
        }

        [OnTrigger]
        private void OnSetDefaults()
        {
            // Amend the message in the model
            if (Model != null)
            {
                Model.MessageRain = "Using Default Settings";
            }
            else
            {
                // Handle the case where Model is null
                // Log or handle the error accordingly
                Mod.DebugLog("Model is null");
                return; // Exit the method early
            }

            // Check if _weatherSystem and _planetarySystem are not null before accessing their members
            if (_weatherSystem._climateSystem != null)
            {
                // Update the time in _planetarySystem
                _weatherSystem._climateSystem.precipitation.overrideState = false;
                _weatherSystem._climateSystem.cloudiness.overrideState = false;
                _weatherSystem._climateSystem.temperature.overrideState = false;
                Model.TemperatureOverride = false;


                Mod.DebugLog("Sun successfully set");
            }
            else
            {
                // Handle the case where _weatherSystem or _planetarySystem is null
                Mod.DebugLog("Planetary or Weather System is null");
            }

            // Trigger an update (assuming TriggerUpdate() is a valid method)
            TriggerUpdate();
        }







        [OnTrigger]
        private void OnSetNight()
        {
            // Amend the message in the model
            if (Model != null)
            {
                Model.Message = "Night Time";
            }
            else
            {
                // Handle the case where Model is null
                // Log or handle the error accordingly
                Mod.DebugLog("Model is null");
                return; // Exit the method early
            }

            // Check if _weatherSystem and _planetarySystem are not null before accessing their members
            if (_weatherSystem != null && _weatherSystem._planetarySystem != null)
            {
                // Update the time in _planetarySystem
                _weatherSystem._planetarySystem.overrideTime = true;
                _weatherSystem._planetarySystem.time = 0f;
                Mod.DebugLog("Night successfully set");
            }
            else
            {
                // Handle the case where _weatherSystem or _planetarySystem is null
                Mod.DebugLog("Planetary or Weather System is null");
            }

            // Trigger an update (assuming TriggerUpdate() is a valid method)
            TriggerUpdate();
        }

        [OnTrigger]
        private void OnSetDefault()
        {
            // Amend the message in the model
            if (Model != null)
            {
                Model.Message = "Using Default Settings";
            }
            else
            {
                // Handle the case where Model is null
                // Log or handle the error accordingly
                Mod.DebugLog("Model is null");
                return; // Exit the method early
            }

            // Check if _weatherSystem and _planetarySystem are not null before accessing their members
            if (_weatherSystem != null && _weatherSystem._planetarySystem != null)
            {
                // Update the time in _planetarySystem
                _weatherSystem._planetarySystem.overrideTime = false;
                Mod.DebugLog("Night successfully set");
            }
            else
            {
                // Handle the case where _weatherSystem or _planetarySystem is null
                Mod.DebugLog("Planetary or Weather System is null");
            }

            // Trigger an update (assuming TriggerUpdate() is a valid method)
            TriggerUpdate();
        }

        [OnTrigger]
        private void OnSetDay()
        {
            // Amend the message in the model
            if (Model != null)
            {
                Model.Message = "Day Time ";
            }
            else
            {
                // Handle the case where Model is null
                // Log or handle the error accordingly
                Mod.DebugLog("Model is null");
                return; // Exit the method early
            }

            // Check if _weatherSystem and _planetarySystem are not null before accessing their members
            if (_weatherSystem != null && _weatherSystem._planetarySystem != null)
            {
                // Update the time in _planetarySystem
                _weatherSystem._planetarySystem.overrideTime = true;
                _weatherSystem._planetarySystem.time = 12f;
                Mod.DebugLog("Night successfully set");
            }
            else
            {
                // Handle the case where _weatherSystem or _planetarySystem is null
                Mod.DebugLog("Planetary or Weather System is null");
            }

            // Trigger an update (assuming TriggerUpdate() is a valid method)
            TriggerUpdate();
        }

        [OnTrigger]
        private void OnToggleVisible( )
        {
            Model.IsVisible = true;
            TriggerUpdate( );
        }
    }
}
