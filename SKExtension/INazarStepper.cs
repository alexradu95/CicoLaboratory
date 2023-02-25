using StereoKit.Framework;

namespace Nazar.Framework;

public interface INazarStepper : IStepper
{
    Type GetConfigUi();
}