using Nazar.Core.Extensions.Passthrough;
using Nazar.Core.Mods;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Core;

public class App : IStepper
{
    public static PassthroughService passthrough;

    public SKSettings Settings => new SKSettings
    {
        appName = "nazar.OS",
        assetsFolder = "Assets",
        displayPreference = DisplayMode.MixedReality
    };

    public bool Enabled => true;

    public App()
    {
        passthrough = SK.AddStepper<PassthroughService>();
    }
    public bool Initialize()
    {
        return true;
    }

    public void Step()
    {

    }

    public void Shutdown()
    {
        throw new System.NotImplementedException();
    }
}