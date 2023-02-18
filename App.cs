using CicoLaboratory.Content;
using CicoLaboratory.Content.Demos;

using StereoKit;

namespace CicoLaboratory;

public class App
{
    public static PassthroughFBExt passthrough;

    public SKSettings Settings => new SKSettings
    {
        appName = "SKTemplate_Maui",
        assetsFolder = "Assets",
        displayPreference = DisplayMode.MixedReality
    };

    public App()
    {
        passthrough = SK.AddStepper<PassthroughFBExt>();
    }

    public void Init()
    {
        SK.AddStepper<FeatureActivatorMenu>();
        //SK.AddStepper<ControllerLocomotion>();
    }

    public void Step()
    {

    }
}