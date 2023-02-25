using Nazar.Core.Features;
using Nazar.Core.Passthrough;

using StereoKit;
using StereoKit.Framework;

namespace Nazar.Core
{
    public class NazarCore : IStepper
    {

        internal static FeaturesManager FeaturesManager;

        public NazarCore()
        {
            // We build the feature manager
            FeaturesManager = SK.AddStepper<FeaturesManager>();

            // We add the passthrough in the constructor, because it need to be initialized before Initialize method is called
            FeaturesManager.AddNewFeature(typeof(PassthroughExtension));
        }

        public bool Enabled => true;
        public bool Initialize() => true;
        public void Shutdown() {}
        public void Step() { }
        
    }
}