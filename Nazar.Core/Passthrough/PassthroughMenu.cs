using System;
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
        bool toggle = CoreFeaturesState.PassthroughExtension.EnabledPassthrough;
        UI.Label(CoreFeaturesState.PassthroughExtension.Available
            ? "Passthrough EXT available!"
            : "No passthrough EXT available :(");
        if (UI.Toggle("Passthrough", ref toggle))
            CoreFeaturesState.PassthroughExtension.EnabledPassthrough = toggle;
        UI.WindowEnd();
    }
}