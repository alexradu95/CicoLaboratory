using System;
using NazAR.Core.Passthrough;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Core.Passthrough;

public class PassthroughMenu : IStepper
{
    private Pose windowPose = new(0.5f, 0, -0.5f, Quat.LookDir(-1, 0, 1));

    public bool Enabled => throw new NotImplementedException();

    public bool Initialize()
    {
        return true;
    }

    public void Shutdown()
    {
    }

    public void Step()
    {
        UI.WindowBegin("Passthrough Settings", ref windowPose);
        bool toggle = PassthroughExtension.PassthroughCore.EnabledPassthrough;
        UI.Label(PassthroughExtension.PassthroughCore.Available
            ? "Passthrough EXT available!"
            : "No passthrough EXT available :(");
        if (UI.Toggle("Passthrough", ref toggle))
            PassthroughExtension.PassthroughCore.EnabledPassthrough = toggle;
        UI.WindowEnd();
    }
}