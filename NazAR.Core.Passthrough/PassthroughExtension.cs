using StereoKit;
using StereoKit.Framework;
using System;
using NazAR.Common;

namespace NazAR.Core.Passthrough
{
    public class PassthroughExtension : IUiStepper
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

        Type IUiStepper.GetUserInterface()
        {
            return typeof(PassthroughMenu);
        }
    }
}
