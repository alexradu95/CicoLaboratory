//using Nazar.Core.Mods;
using Nazar.Core.Mods;
using StereoKit;
using StereoKit.Framework;
using VRWorld;

namespace Nazar;

public class App : IStepper
{
    public SKSettings Settings => new SKSettings
    {
        appName = "nazar.OS",
        assetsFolder = "Assets",
        displayPreference = DisplayMode.MixedReality
    };

    public bool Enabled => true;

    public App()
    {
        SK.AddStepper<CoreFeatures>();
    }
    public bool Initialize() => true;
    public void Step() { }
    public void Shutdown() { }
}