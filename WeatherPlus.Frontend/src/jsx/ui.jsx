import React from "react";

const WeatherPlusContainer = ({ react, setupController }) => {
    const { Icon, Button, TabModal } = window.$_gooee.framework;

    const { model, update, trigger } = setupController();

    const { Slider } = window.$_gooee.framework;
    const { RadioGroup } = window.$_gooee.framework;
    const { FormCheckBox } = window.$_gooee.framework;
    const Temperature = model.Temperature;
    const RainAmount = model.RainAmount;
    const TemperatureOverride = model.TemperatureOverride;
    const RainOverride = model.RainOverride;
    const CloudsOverride = model.CloudOverride;
    const CloudsAmount = model.CloudsAmount;
    const Time = model.Time;

   
    

    const onTimeChanged = (selected) => {
        update("Time", selected);
        trigger("OnSetTime");
    };
    const onTemperatureChanged = (selected) => {
        update("Temperature", selected);
        trigger("OnSetTemperature");
    };
    const onRainChanged = (selected) => {
        update("RainAmount", (selected));  
        trigger("onRainChanged");
    };
    const onCloudsChanged = (selected) => {
        update("CloudsAmount", (selected));
        trigger("onCloudsChanged");
    };
    const toSliderValue = () => {
        return Temperature;
    };
    const toSliderValueRain = () => {
        return RainAmount;
    };
    const toSliderValueClouds = () => {
        return CloudsAmount;
    };
    const toSliderValueTime = () => {
        return Time;
    };

    const onTemperatureOverride = (value) => {
        update("TemperatureOverride", value);
        trigger("OnOverrideTemperature");
    };

    const OnRainOverride = (value) => {
        update("RainOverride", value);
        trigger("OnRainOverride");
    };
    const OnCloudsOverride = (value) => {
        update("CloudsOverride", value);
        trigger("OnCloudsOverride");
    };



    const tabs = [
        {
            name: "Time of Day",
            label: <div className="tab-label"> {engine.translate("WP_TimeOfDay")}
                <Icon icon="clock" fa />
            </div>,
            content:
                <div>
                    <h3 className="mb-2">{model.Message}</h3>
                    <Button style={{ textAlign: 'center' }} color="primary" onClick={() => trigger("OnSetNight")}><Icon icon="moon" fa />&nbsp;{engine.translate("WP_Night")}</Button>
                    <span> </span>
                    <Button style={{ textAlign: 'center' }} color="primary" onClick={() => trigger("OnSetDay")}><Icon icon="sun" fa />&nbsp;{engine.translate("WP_Day")}</Button>
                    <span> </span>
                    <h4>{engine.translate("WP_SetTimeOfDay")}</h4>
                    <span> </span>
                    <Slider
                        value={toSliderValueTime}
                        onValueChanged={(value) => onTimeChanged(value)}
                    />
                    <span> </span>
                    <Button style={{ textAlign: 'center' }} color="primary" onClick={() => trigger("OnSetDefault")}><Icon icon="hand" fa />&nbsp;{engine.translate("WP_Default")}</Button>
                </div>
        },
        {
            name: "Weather",
            label: <div className="tab-label"> {engine.translate("WP_Weather")}
                <Icon icon="sun" fa />
            </div>,
            content:
                <div>
                    <h3 className="mb-2">{model.MessageRain}</h3>
                    <Button style={{ textAlign: 'center' }} color="primary" onClick={() => trigger("OnSetRain")}><Icon icon="gem" fa />&nbsp;{engine.translate("WP_Rain")}</Button>
                    <span> </span>
                    <Button style={{ textAlign: 'center' }} color="primary" onClick={() => trigger("OnSetSnow")}><Icon icon="snowflake" fa />&nbsp;{engine.translate("WP_Snow")}</Button>
                    <span> </span>
                    <Button style={{ textAlign: 'center' }} color="primary" onClick={() => trigger("OnSetSun")}><Icon icon="sun" fa />&nbsp;{engine.translate("WP_Sun")}</Button>
                    <span> </span>
                    <Button style={{ textAlign: 'center' }} color="primary" onClick={() => trigger("OnSetDefaults")}><Icon icon="hand" fa />&nbsp;{engine.translate("WP_Default")}</Button>
                </div>
        },
        {
            name: "WeatherAdv",
            label: <div className="tab-label"> {engine.translate("WP_WeatherAdvanced")}
                <Icon icon="sun" fa />
            </div>,
            content:
                <div>
                    <h3 className="mb-2">{model.MessageAdvanced}</h3>
                    <span> </span>
                    <h2 className="mb-2">{engine.translate("WP_ManualControl")}</h2>
                    <span> </span>
                    <h4>{engine.translate("WP_SetTemperature")}</h4>
                    <span> </span>
                    <FormCheckBox checked={model.TemperatureOverride} onToggle={value => onTemperatureOverride(value)} />
                    <span> </span>
                    <Slider
                        value={toSliderValue}
                        onValueChanged={(value) => onTemperatureChanged(value - 50)}
                    />
                    <span> </span>
                    <h4>{engine.translate("WP_SetRainSnowIntensity")}</h4>
                    <span> </span>
                    <FormCheckBox checked={model.RainOverride} onToggle={value => OnRainOverride(value)} />
                    <span> </span>
                    <Slider
                        value={toSliderValueRain}
                        onValueChanged={(value) => onRainChanged(value)}
                    />
                    <span> </span>
                    <h4>{engine.translate("WP_SetCloudsIntensity")}</h4>
                    <span> </span>
                    <FormCheckBox checked={model.CloudsOverride} onToggle={value => OnCloudsOverride(value)} />
                    <span> </span>
                    <Slider
                        value={toSliderValueClouds}
                        onValueChanged={(value) => onCloudsChanged(value)}
                    />
                </div>
        }
    ];

    const closeModal = () => {
        update("IsVisible", false);
        engine.trigger("audio.playSound", "close-panel", 1);
    };

    return model.IsVisible ? <TabModal size="lg" tabs={tabs} onClose={closeModal} fixed className="WeatherPlusContainer" /> : null;
};

window.$_gooee.register("weatherplus", "WeatherPlusContainer", WeatherPlusContainer, "main-container", "weatherplus");
