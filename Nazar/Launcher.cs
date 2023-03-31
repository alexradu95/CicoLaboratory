using Nazar.Framework;
using StereoKit;

namespace NazAR
{
    public class Launcher : Node
    {

        private readonly HierarchyExample hierarchyExample = new HierarchyExample();

        public override bool Initialize()
        {
            AddChild(hierarchyExample);

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