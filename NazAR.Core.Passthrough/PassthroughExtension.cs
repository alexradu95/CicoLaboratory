using Nazar.Core.Passthrough;
using StereoKit;
using StereoKit.Framework;
using System;

namespace NazAR.Core.Passthrough
{
    public class PassthroughExtension : IStepper
    {
        public bool Enabled => throw new NotImplementedException();


        #region IStepper children

        public static PassthroughCore PassthroughCore;
        public static PassthroughMenu PassthroughMenu;

        #endregion

        public PassthroughExtension()
        {
            PassthroughCore = SK.AddStepper<PassthroughCore>();
            PassthroughMenu = SK.AddStepper<PassthroughMenu>();
        }

        public bool Initialize() => true;

        public void Shutdown() {}

        public void Step() {}
    }
}
