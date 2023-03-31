using Addons;
using Core;
using Nazar.Framework;
using StereoKit;

namespace NazAR
{
    public class Launcher : Node
    {

        private readonly CoreModule coreModule = new CoreModule();
        private readonly AddonsModule addonsModule = new AddonsModule();

        public override bool Initialize()
        {
            AddChild(coreModule);
            AddChild(addonsModule);

            return base.Initialize();
        }

        public override void Step()
        {
            base.Step();
        }

        public SKSettings Settings => new()
        {
            appName = "nazar.OS",
            assetsFolder = "Assets",
            displayPreference = DisplayMode.MixedReality
        };

    }
}