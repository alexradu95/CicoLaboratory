using Addons;
using Core;
using Framework;
using StereoKit;

namespace NazAR
{
    public class Launcher : Node
    {
        public override bool Initialize()
        {
            AddChild(new CoreModule());
            AddChild(new AddonsModule());

            return base.Initialize();
        }

        public SKSettings Settings => new()
        {
            appName = "nazar.OS",
            assetsFolder = "Assets",
            displayPreference = DisplayMode.MixedReality
        };

    }
}