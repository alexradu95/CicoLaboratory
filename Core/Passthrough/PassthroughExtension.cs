using Microsoft.VisualBasic;
using Nazar.Framework;
using StereoKit;

namespace Nazar.Core.Passthrough
{
    public class PassthroughExtension : Node, IConfigurableMenu
    {
        PassthroughCore passthrough;

        private Pose windowPose = new(0.5f, 0, -0.5f, Quat.LookDir(-1, 0, 1));

        public override bool Enabled => true;

        public PassthroughExtension()
        {
            passthrough = (PassthroughCore) AddChild(typeof(PassthroughCore));
        }

        public override bool Initialize() => true;

        public override void Step()
        {
            DrawMenu();
        }

        public void DrawMenu()
        {
            UI.WindowBegin("Passthrough Settings", ref windowPose);
            bool toggle = passthrough.EnabledPassthrough;
            UI.Label(passthrough.Available ? "Passthrough EXT available!" : "No passthrough EXT available :(");
            if (UI.Toggle("Passthrough", ref toggle))
                passthrough.EnabledPassthrough = toggle;
            UI.WindowEnd();
        }
    }
}
