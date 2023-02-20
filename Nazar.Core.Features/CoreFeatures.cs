using Nazar.Core.Features;
using Nazar.Core.Features.Passthrough;
using StereoKit;
using StereoKit.Framework;

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

            return true;

        }
       

        public void Shutdown() { }
        public void Step() { }
    }
}
