using Nazar.Application;
using StereoKit;
using StereoKit.Framework;

namespace NazAR
{
    public class Launcher : IStepper
    {
        public Launcher()
        {
            SK.AddStepper<NazarApplication>();
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