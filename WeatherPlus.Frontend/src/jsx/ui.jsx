import React from "react";

const ExampleMainContainer = ({ react, setupController }) => {
    const { Icon, Button, TabModal } = window.$_gooee.framework;

    const { model, update, trigger } = setupController();

    const { Slider } = window.$_gooee.framework;
    const { RadioGroup } = window.$_gooee.framework;
    const { FormCheckBox } = window.$_gooee.framework;
    const Temperature = model.Temperature;
    const TemperatureOverride = model.TemperatureOverride;


    const onTemperatureChanged = (selected) => {
        update("Temperature", selected);
        trigger("OnSetTemperature");
    };
    const toSliderValue = () => {
        return Temperature;
    };

    const onTemperatureOverride = (value) => {
        update("TemperatureOverride", value);
        trigger("OnOverrideTemperature");
    };

    const tabs = [
        {
            name: "Time of Day",
            label: <div> Time of Day
                <Icon icon="clock" fa />
            </div>,
            content:
                <div>
                    <h4 className="mb-2">{model.Message}</h4>
                    <Button color="primary" onClick={() => trigger("OnSetNight")}>Set Night</Button> 
                    <span> </span>
                    <Button color="primary" onClick={() => trigger("OnSetDay")}>Set Day</Button>
                    <span> </span>
                    <Button color="primary" onClick={() => trigger("OnSetDefault")}>Use Default</Button>
                    <span> </span>
                    <h4>Set Temperature</h4>
                    <span> </span>
                    <FormCheckBox checked={model.TemperatureOverride} onToggle={value => onTemperatureOverride(value)} />
                    <span> </span>    
                    <Slider
                        value={toSliderValue}
                        onValueChanged={(value) => onTemperatureChanged(value - 50)}
                    />

                </div>

                
        }, {
            name: "Weather",
            label: <div> Weather
                <Icon icon="sun" fa />
            </div>,
            content:
                <div>
                    <h4 className="mb-2">{model.MessageRain}</h4>
                    <Button color="primary" onClick={() => trigger("OnSetRain")}>Rain</Button>
                    <span> </span>
                    <Button color="primary" onClick={() => trigger("OnSetSnow")}>Snow</Button>
                    <span> </span>
                    <Button color="primary" onClick={() => trigger("OnSetSun")}>Sun</Button>
                    <span> </span>
                    <Button color="primary" onClick={() => trigger("OnSetDefaults")}>Default</Button>
                </div>
        }]; 

    const closeModal = () => {
        update("IsVisible", false);
        engine.trigger("audio.playSound", "close-panel", 1);
    };

    return model.IsVisible ? <TabModal size="sm" tabs={tabs} onClose={closeModal} fixed /> : null;
};

window.$_gooee.register("weatherplus", "ExampleMainContainer", ExampleMainContainer, "main-container", "weatherplus");