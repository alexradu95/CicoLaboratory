using Nazar.Core.Passthrough;
using Nazar.Features.AI;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Core;

public class CoreFeatures : IStepper
{
    public CoreFeatures()
    {
        // We instantiate it in the constructor because the Pass-through Service must be initialized before SK
        CoreFeaturesState.PassthroughExtension = SK.AddStepper<PassthroughExtension>();
        CoreFeaturesState.CoreFeatures = SK.AddStepper<CoreFeaturesMenu>();
    }

    public bool Enabled => true;

    public bool Initialize()
    {

        CoreFeaturesState.ToggleableFeatures.Add(typeof(AiAssistant));
        return true;
    }


    public void Shutdown()
    {
    }

    public void Step()
    {
    }
}