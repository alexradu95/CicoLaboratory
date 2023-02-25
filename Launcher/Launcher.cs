using StereoKit;
using StereoKit.Framework;
using Nazar.Application;

namespace NazAR
{
    public class Launcher : IStepper
    {
        public Launcher()
        {
            SK.AddStepper<NazarApp>();
        }

        public SKSettings Settings => new()
        {
            appName = "nazar.OS",
            assetsFolder = "Assets",
            displayPreference = DisplayMode.MixedReality
        };

        public bool Enabled => true;

        public bool Initialize()
        {
            return true;
        }

        public void Step()
        {
        }

        public void Shutdown()
        {
        }
    }
}