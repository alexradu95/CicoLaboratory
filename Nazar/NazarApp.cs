using Nazar.Core;
using Nazar.Extensions;
using StereoKit;
using StereoKit.Framework;

namespace Nazar
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