using CicoLaboratory.Content;
using CicoLaboratory.Content.Demos;
using CicoLaboratory.Content.Environment;
using CicoLaboratory.Content.Locomotion;
using CicoLaboratory.Features;
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
        SK.AddStepper<ShowImage>();
        //SK.AddStepper<ControllerLocomotion>();
    }

    public void Step()
    {

    }
}