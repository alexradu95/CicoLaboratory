using Nazar;
using Nazar.Framework;
using StereoKit;
using StereoKit.Framework;

namespace NazAR
{
    public class Launcher : Node
    {
        public Launcher()
        {
            SK.AddStepper<HierarchyExample>();
        }

        public SKSettings Settings => new()
        {
            appName = "nazar.OS",
            assetsFolder = "Assets",
            displayPreference = DisplayMode.MixedReality
        };

    }
}