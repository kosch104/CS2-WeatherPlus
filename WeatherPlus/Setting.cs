using Colossal.IO.AssetDatabase;
using Colossal;
using Game.Modding;
using Game.Prefabs;
using Game.Settings;
using Game.UI.Widgets;
using Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherPlus.Models;
using Gooee.Plugins;
using WeatherPlus.UI;
using Gooee;
using Game.Simulation;
using UnityEngine.InputSystem;



namespace WeatherPlus
{
    [FileLocation(nameof(WeatherPlus))]
    [SettingsUIGroupOrder(kButtonGroup)]
    [SettingsUIShowGroupName(kButtonGroup)]
    public class Setting : ModSetting
    {
        private Mod _mod;
        public const string kSection = "Main";
        private DanielsWeatherSystem _weatherSystem;
        public WeatherPlusModel _model;

        public const string kButtonGroup = "Button";


        public Setting(IMod mod, DanielsWeatherSystem weatherSystem) : base(mod)
        {
            _mod = (Mod)mod;
            _weatherSystem = weatherSystem;
            

            /*if (Time == 0)
            {
                Time = 1;
            }
            if (CloudAmount == 0)
            {
                CloudAmount = 1;
            }
            if (RainAmount == 0) 
            { 
                RainAmount = 1;
            }
            if (Temperature == 0)
            {
                Temperature = 1;
            }*/
            
            
        }


        public override void SetDefaults()
        {
            //throw new System.NotImplementedException();
            Time = 1;
            CloudAmount = 1;
            Temperature = 1;
            RainAmount = 1;
            OverrideTime = false;
            CloudsOverride = false;
            RainOverride = false;
            TemperatureOverride = false;

        }

        [SettingsUISection(kSection, kButtonGroup)]
        public bool Button { get; set; }


        [SettingsUISearchHidden]
        [SettingsUIHidden]
        [SettingsUISection(kSection, kButtonGroup)]
        public bool OverrideTime { get; set; } 
        [SettingsUISearchHidden]
        [SettingsUIHidden]
        [SettingsUISection(kSection, kButtonGroup)]
        public float Time { get; set; } 
        [SettingsUISearchHidden]
        [SettingsUIHidden]
        [SettingsUISection(kSection, kButtonGroup)]
        public float CloudAmount { get; set; } 
        [SettingsUISearchHidden]
        [SettingsUIHidden]
        [SettingsUISection(kSection, kButtonGroup)]
        public bool CloudsOverride { get; set; } 
        [SettingsUISearchHidden]
        [SettingsUIHidden]
        [SettingsUISection(kSection, kButtonGroup)]
        public bool RainOverride { get; set; } 
        [SettingsUISearchHidden]
        [SettingsUIHidden]
        [SettingsUISection(kSection, kButtonGroup)]
        public float RainAmount { get; set; } 
        [SettingsUISearchHidden]
        [SettingsUIHidden]
        [SettingsUISection(kSection, kButtonGroup)]
        public bool TemperatureOverride { get; set; } 
        [SettingsUISearchHidden]
        [SettingsUIHidden]
        [SettingsUISection(kSection, kButtonGroup)]
        public int Temperature { get; set; } 


       


        public class LocaleEN : IDictionarySource
        {
            private readonly Setting m_Setting;
            public LocaleEN(Setting setting)
            {
                m_Setting = setting;
            }
            public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
            {
                return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Weather Plus" },
                { m_Setting.GetOptionTabLocaleID(Setting.kSection), "Main" },

                { m_Setting.GetOptionGroupLocaleID(Setting.kButtonGroup), "Settings no longer used" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Button)), "Settings no longer used" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Button)), $"Please refer to the briefcase in the top left of your screen in game and select Weather Plus" },

            };
            }

            public void Unload()
            {


            }
        }
    }
}
