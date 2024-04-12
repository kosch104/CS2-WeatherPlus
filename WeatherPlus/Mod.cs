﻿using HarmonyLib;
using Gooee.Plugins.Attributes;
using Gooee.Plugins;
using WeatherPlus.UI;
using Colossal.Logging;
using Game.Modding;
using Game;
using Game.Simulation;
using System.Threading.Tasks;
using Colossal.IO.AssetDatabase;
using Colossal.Serialization.Entities;
using Game.SceneFlow;
using Unity.Entities;
using Gooee;
using WeatherPlus;
using Game.Settings;
using Game.Prefabs;
using static WeatherPlus.Setting;




namespace WeatherPlus
{
    public class Mod : IMod
    {
        public static ILog _log = LogManager.GetLogger("WeatherPlus").SetShowsErrorsInUI(false);
        private const string HARMONY_ID = "Daniel_WeatherPlus";
        private static Harmony _harmony;
        public DanielsWeatherSystem _weatherSystem;
        public static Setting m_Setting;
        public ClimateSystem _climateSystem;

        



        //public DanielsWeatherSystem _weatherSystem;
        public void OnLoad(UpdateSystem updateSystem)
        {
            _harmony = new Harmony(HARMONY_ID);
            _harmony.PatchAll();
            _log.Info("Loaded WeatherPlus Mod");
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                _log.Info($"Current mod asset at {asset.path}");

           /* m_Setting = new Setting(this, _weatherSystem);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));

            AssetDatabase.global.LoadSettings(nameof(WeatherPlus), m_Setting, new Setting(this, _weatherSystem));*/

            if (_weatherSystem == null)
            {
                _weatherSystem = new DanielsWeatherSystem(this);
            }

            if (m_Setting == null)
            {
                m_Setting = new Setting(this, _weatherSystem);
                m_Setting.RegisterInOptionsUI();
                GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));

                AssetDatabase.global.LoadSettings(nameof(WeatherPlus), m_Setting, new Setting(this, _weatherSystem));
                Mod._log.Info("Settings Found");
            }
            else
            {
                Mod._log.Info("Settings Not Found");
            }

            World.DefaultGameObjectInjectionWorld.AddSystemManaged(_weatherSystem);

            



            updateSystem.UpdateAt<WeatherPlusController>(SystemUpdatePhase.MainLoop);
            updateSystem.UpdateAt<WeatherPlusController>(SystemUpdatePhase.ApplyTool);
        }


        public void OnDispose()
        {
            // DO ANY CLEANUP HERE!

            _harmony?.UnpatchAll(HARMONY_ID);
            _log.Info("Unloaded WeatherPlus Mod");

            _log.Info(nameof(OnDispose));
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }

            
        }

        public static void DebugLog(string message)
        {
            _log.Info(message);
        }

    }



    [ControllerTypes(typeof(WeatherPlusController))]
    [PluginToolbar(typeof(WeatherPlusController), "OnToggleVisible", "Weather Plus", "Media/Game/Climate/Sun.svg")]
    public class WeatherPlusPlugin : IGooeePluginWithControllers
    {
        public string Name => "WeatherPlus";

        public string ScriptResource => "WeatherPlus.Resources.ui.js";

        public string StyleResource => "WeatherPlus.Frontend.src.style.style.scss";

        public IController[] Controllers
        {
            get;
            set;
        }
    }

    public partial class DanielsWeatherSystem : GameSystemBase
    {
        //public ClimateSystem _climateSystem;
        public PlanetarySystem _planetarySystem;
        public ClimateSystem _climateSystem;
        public Mod _mod;
        private bool isInitialized;

        public DanielsWeatherSystem(Mod mod)
        {
            _mod = mod;
        }


        protected override void OnCreate()
        {
            base.OnCreate();
            //_climateSystem = World.GetExistingSystemManaged<ClimateSystem>();
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            if (!mode.IsGameOrEditor())

                return;



            if (_climateSystem == null && Mod.m_Setting != null && _planetarySystem == null)
            {
                //_climateSystem = World.GetExistingSystemManaged<ClimateSystem>();
                _climateSystem = World.GetExistingSystemManaged<ClimateSystem>();
                Mod._log.Info("Climate System Found on Mod.cs - Test");
                _planetarySystem = World.GetExistingSystemManaged<PlanetarySystem>();
                Mod._log.Info("Planetary System Found on Mod.cs");

                _climateSystem.temperature.overrideState = Mod.m_Setting.TemperatureOverride;
                _climateSystem.temperature.overrideValue = Mod.m_Setting.Temperature;
                _climateSystem.precipitation.overrideState = Mod.m_Setting.RainOverride;
                _climateSystem.precipitation.overrideValue = Mod.m_Setting.RainAmount;
                _climateSystem.cloudiness.overrideState = Mod.m_Setting.CloudsOverride;
                _climateSystem.cloudiness.overrideValue = Mod.m_Setting.CloudAmount;
                _planetarySystem.overrideTime = Mod.m_Setting.OverrideTime;
                _planetarySystem.time = Mod.m_Setting.Time;
                Mod._log.Info("Restored Previous Settings");
                Mod._log.Info("Temperature: " + Mod.m_Setting.Temperature);
                Mod._log.Info("Temperature Override: " + Mod.m_Setting.TemperatureOverride);
                Mod._log.Info("Rain Override: " + Mod.m_Setting.RainOverride);
                Mod._log.Info("Rain Amount: " + Mod.m_Setting.RainAmount);
                Mod._log.Info("Clouds Override: " + Mod.m_Setting.CloudsOverride);
                Mod._log.Info("Clouds Amount: " + Mod.m_Setting.CloudAmount);
                Mod._log.Info("Time Override: " + Mod.m_Setting.OverrideTime);
                Mod._log.Info("Time: " + Mod.m_Setting.Time);


            }
            else if (Mod.m_Setting == null)
            {
                Mod._log.Info("Settings is NULL");
            }
            else
            {
                Mod._log.Info("Climate System Not Found on Mod.cs");
            }



            isInitialized = true;

        }


        protected override void OnUpdate()
        {

            if (!isInitialized || _planetarySystem == null || _climateSystem == null)
                return;


        }


        public void OnGameExit()
        {
            
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}

    
