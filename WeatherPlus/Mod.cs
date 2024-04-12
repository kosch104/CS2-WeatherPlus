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




namespace WeatherPlus
{
    public class Mod : IMod
    {
        public static ILog _log = LogManager.GetLogger("Daniel").SetShowsErrorsInUI(false);
        private const string HARMONY_ID = "Daniel_WeatherPlus";
        private static Harmony _harmony;
        public DanielsWeatherSystem _weatherSystem;


        //public DanielsWeatherSystem _weatherSystem;
        public void OnLoad(UpdateSystem updateSystem)
        {
            _harmony = new Harmony(HARMONY_ID);
            _harmony.PatchAll();
            _log.Info("Loaded WeatherPlus Mod");



            if (_weatherSystem == null)
            {
                _weatherSystem = new DanielsWeatherSystem(this);
            }

            World.DefaultGameObjectInjectionWorld.AddSystemManaged(_weatherSystem);

            updateSystem.UpdateAt<WeatherPlusController>(SystemUpdatePhase.MainLoop);
            updateSystem.UpdateAt<WeatherPlusController>(SystemUpdatePhase.ApplyTool);


            /*if (_weatherSystem == null)
            {
                _weatherSystem = new DanielsWeatherSystem(this);
            }*/
        }


        public void OnDispose()
        {
            // DO ANY CLEANUP HERE!

            _harmony?.UnpatchAll(HARMONY_ID);
            _log.Info("Unloaded WeatherPlus Mod");
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


            if (_planetarySystem == null)
            {
                //_climateSystem = World.GetExistingSystemManaged<ClimateSystem>();
                _planetarySystem = World.GetExistingSystemManaged<PlanetarySystem>();
                Mod._log.Info("Planetary System Found on Mod.cs");
            }

            if (_climateSystem == null)
            {
                //_climateSystem = World.GetExistingSystemManaged<ClimateSystem>();
                _climateSystem = World.GetExistingSystemManaged<ClimateSystem>();
                Mod._log.Info("Climate System Found on Mod.cs");
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
            //isInitialized = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}

    
