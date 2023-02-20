using StereoKit;
using StereoKit.Framework;

namespace Nazar.Core.Mods.Passthrough;

public class PassthroughModUI : IStepper
{
    Pose windowPose = new Pose(0.5f, 0, -0.5f, Quat.LookDir(-1, 0, 1));

    public bool Enabled => throw new System.NotImplementedException();

    public bool Initialize() { return true; }

    public void Shutdown() { }

    public void Step()
    {
        UI.WindowBegin("Passthrough Settings", ref windowPose);
        bool toggle = PassthroughMod.passthrough.EnabledPassthrough;
        UI.Label(PassthroughMod.passthrough.Available
            ? "Passthrough EXT available!"
            : "No passthrough EXT available :(");
        if (UI.Toggle("Passthrough", ref toggle))
            PassthroughMod.passthrough.EnabledPassthrough = toggle;
        UI.WindowEnd();
    }
}