using System;
using Nazar.SKit.Framework;
using StereoKit;

namespace Nazar.Core.Passthrough
{
    public class PassthroughExtension : IConfigurableStepper
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
