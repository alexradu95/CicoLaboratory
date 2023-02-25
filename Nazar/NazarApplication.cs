using Nazar.Core;
using Nazar.Extension;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Application
{
    public class NazarApplication : IStepper
    {
        public NazarApplication()
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