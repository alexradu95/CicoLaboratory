using Nazar.Core.Features;
using Nazar.Core.Mods;
using StereoKit;

namespace Nazar.Core;

public class App
{

    public SKSettings Settings => new SKSettings
    {
        appName = "nazar.OS",
        assetsFolder = "Assets",
        displayPreference = DisplayMode.MixedReality
    };

    public App()
    {

    }

    public void Init()
    {
        SK.AddStepper<CoreMods>();
        SK.AddStepper<ExtensionMods>();
    }

    public void Step()
    {

    }
}