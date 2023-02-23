using Nazar.Extension.AIWorldGenerator;
using Nazar.Extension.Demos;
using Nazar.Extension.Features;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Extension
{
    public class NazarExtension : IStepper
    {

        internal static ExtensionManager ExtensionManager;

        public NazarExtension()
        {
            // We build the extensions manager
            ExtensionManager = SK.AddStepper<ExtensionManager>();

            ExtensionManager.AddNewFeature(typeof(NazarExtensionDemos));

            ExtensionManager.AddNewFeature(typeof(AiAssistant));

        }

        public bool Enabled => true;
        public bool Initialize() => true;
        public void Shutdown() { }
        public void Step() { }

    }
}