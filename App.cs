using CicoLaboratory.Content;
using CicoLaboratory.Content.Environment;
using StereoKit;
using StereoKitTest;
using StereoKitTest.Demos;

namespace CicoLaboratory;

public class App
{
    public SKSettings Settings => new SKSettings
    {
        appName = "SKTemplate_Maui",
        assetsFolder = "Assets",
        displayPreference = DisplayMode.MixedReality
    };

    public void Init()
    {
        SK.AddStepper<FeatureActivatorMenu>();
        SK.AddStepper<ControllerLocomotion>();
    }

    public void Step()
    {

    }
}