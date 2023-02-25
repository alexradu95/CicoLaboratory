using System;
using Nazar.Framework;
using StereoKit;

namespace Nazar.Core.Passthrough
{
    public class PassthroughExtension : INazarStepper
    {
        public bool Enabled => throw new NotImplementedException();

        public static PassthroughCore passthroughCore;

        public Type GetConfigUi()
        {
            return typeof(PassthroughMenu);
        }

        public PassthroughExtension()
        {
            passthroughCore = SK.AddStepper<PassthroughCore>();
        }

        public bool Initialize() => true;

        public void Shutdown() {}

        public void Step() {}

    }
}
