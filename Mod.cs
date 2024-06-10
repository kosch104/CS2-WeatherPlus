using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Unity.Entities;

namespace WeatherPlus;

public class Mod : IMod
{
    public static ILog log = LogManager.GetLogger($"{nameof(WeatherPlus)}.{nameof(Mod)}").SetShowsErrorsInUI(false);
    public WeatherPlusSystem _weatherSystem;
    public static Setting m_Setting;


    public void OnLoad(UpdateSystem updateSystem)
    {
        //if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
        //    log.Info($"Current mod asset at {asset.path}");


        if (_weatherSystem == null)
            _weatherSystem = new WeatherPlusSystem();


        World.DefaultGameObjectInjectionWorld.AddSystemManaged(_weatherSystem);
        WeatherPlusSystem.Instance = _weatherSystem;

        m_Setting = new Setting(this);
        m_Setting.RegisterInOptionsUI();
        GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
        AssetDatabase.global.LoadSettings(nameof(WeatherPlus), m_Setting, new Setting(this));
        m_Setting.Apply();

        updateSystem.UpdateAt<WeatherPlusSystem>(SystemUpdatePhase.MainLoop);
        updateSystem.UpdateAt<WeatherPlusSystem>(SystemUpdatePhase.ApplyTool);
    }


    public void OnDispose()
    {
        log.Info("OnDispose Ran Successfully, set isInitialiszed to FALSE.");
    }
}