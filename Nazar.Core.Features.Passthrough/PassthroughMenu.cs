using StereoKit;
using StereoKit.Framework;

namespace Nazar.Core.Features.Passthrough;

public class PassthroughMenu : IStepper
{
    Pose windowPose = new Pose(0.5f, 0, -0.5f, Quat.LookDir(-1, 0, 1));

    public bool Enabled => throw new System.NotImplementedException();

    public bool Initialize() { return true; }

    public void Shutdown() { }

    public void Step()
    {
        UI.WindowBegin("Passthrough Settings", ref windowPose);
        bool toggle = Passthrough.enabledPassthrough;
        UI.Label(Passthrough.Available
            ? "Passthrough EXT available!"
            : "No passthrough EXT available :(");
        if (UI.Toggle("Passthrough", ref toggle))
            Passthrough.enabledPassthrough = toggle;
        UI.WindowEnd();
    }
}