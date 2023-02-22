using Nazar.Core.Passthrough;
using StereoKit;
using StereoKit.Framework;
using System;

namespace NazAR.Core.Passthrough
{
    public class PassthroughExtension : INazarStepper
    {
        public bool Enabled => throw new NotImplementedException();

        public static PassthroughCore passthroughCore;

        public PassthroughExtension()
        {
            passthroughCore = SK.AddStepper<PassthroughCore>();
        }

        public bool Initialize() => true;

        public void Shutdown() {}

        public void Step() {}

        Type INazarStepper.GetUserInterface()
        {
            return typeof(PassthroughMenu);
        }
    }
}
