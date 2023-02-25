using Nazar.Core;
using Nazar.Extension;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Application
{
    public class NazarApp : IStepper
    {
        public NazarApp()
        {
            SK.AddStepper<NazarCore>();
            SK.AddStepper<NazarExtension>();
        }

        public bool Enabled => true;

        public bool Initialize() => true;


        public void Shutdown() { }

        public void Step() { }

    }
}