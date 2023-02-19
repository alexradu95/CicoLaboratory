using CicoLaboratory.Content;
using CicoLaboratory.Content.Demos;

using StereoKit;

namespace Nazar.Core;

public class App
{
    public static PassthroughFBExt passthrough;

    public SKSettings Settings => new SKSettings
    {
        appName = "nazar.OS",
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
    }

    public void Step()
    {

    }
}