using Nazar.Core.Features;
using Nazar.Core.Features.Passthrough;
using StereoKit;
using StereoKit.Framework;
using VRWorld;

namespace Nazar.Core.Mods
{
    public class CoreFeatures : IStepper
    {
        public bool Enabled => true;

        public CoreFeatures()
        {
            // We instantiate it in the constructor because the PassthroughService must be initialized before SK
            CoreFeaturesState.PassthroughExtension = SK.AddStepper<PassthroughExtension>();
            CoreFeaturesState.CoreFeatures = SK.AddStepper<CoreFeaturesMenu>();
        }

        public bool Initialize() {

            CoreFeaturesState.ToggleableFeatures.Add(typeof(AiAssistant));
            return true;

        }
       

        public void Shutdown() { }
        public void Step() { }
    }
}
