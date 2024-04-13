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
            label: <div className="tab-label"> Time of Day
                <Icon icon="clock" fa />
            </div>,
            content:
                <div>
                    <h3 className="mb-2">{model.Message}</h3>
                    <Button style={{ textAlign: 'center' }} color="primary" onClick={() => trigger("OnSetNight")}><Icon icon="moon" fa />&nbsp;Night</Button>
                    <span> </span>
                    <Button style={{ textAlign: 'center' }} color="primary" onClick={() => trigger("OnSetDay")}><Icon icon="sun" fa />&nbsp;Day</Button>
                    <span> </span>
                    <Button style={{ textAlign: 'center' }} color="primary" onClick={() => trigger("OnSetDefault")}><Icon icon="hand" fa />&nbsp;Default</Button>
                </div>
        },
        {
            name: "Weather",
            label: <div className="tab-label"> Weather
                <Icon icon="sun" fa />
            </div>,
            content:
                <div>
                    <h3 className="mb-2">{model.MessageRain}</h3>
                    <Button style={{ textAlign: 'center' }} color="primary" onClick={() => trigger("OnSetRain")}><Icon icon="gem" fa />&nbsp;Rain</Button>
                    <span> </span>
                    <Button style={{ textAlign: 'center' }} color="primary" onClick={() => trigger("OnSetSnow")}><Icon icon="snowflake" fa />&nbsp;Snow</Button>
                    <span> </span>
                    <Button style={{ textAlign: 'center' }} color="primary" onClick={() => trigger("OnSetSun")}><Icon icon="sun" fa />&nbsp;Sun</Button>
                    <span> </span>
                    <Button style={{ textAlign: 'center' }} color="primary" onClick={() => trigger("OnSetDefaults")}><Icon icon="hand" fa />&nbsp;Default</Button>
                </div>
        },
        {
            name: "WeatherAdv",
            label: <div className="tab-label"> Weather (Advanced)
                <Icon icon="sun" fa />
            </div>,
            content:
                <div>
                    <h3 className="mb-2">{model.MessageAdvanced}</h3>
                    <span> </span>
                    <h2 className="mb-2">Manual Control</h2>
                    <span> </span>
                    <h4>Set Temperature</h4>
                    <span> </span>
                    <FormCheckBox checked={model.TemperatureOverride} onToggle={value => onTemperatureOverride(value)} />
                    <span> </span>
                    <Slider
                        value={toSliderValue}
                        onValueChanged={(value) => onTemperatureChanged(value - 50)}
                    />
                    <span> </span>
                    <h4>Set Rain/Snow Intensity</h4>
                    <span> </span>
                    <FormCheckBox checked={model.RainOverride} onToggle={value => OnRainOverride(value)} />
                    <span> </span>
                    <Slider
                        value={toSliderValueRain}
                        onValueChanged={(value) => onRainChanged(value)}
                    />
                    <span> </span>
                    <h4>Set Clouds Intensity</h4>
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
