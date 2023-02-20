using StereoKit;
using StereoKit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nazar.Core.Mods.Passthrough
{
    public class PassthroughMod : IStepper
    {

        public static PassthroughFunctionality passthrough;

        public bool Enabled => true;

        public bool Initialize()
        {
            passthrough = SK.AddStepper<PassthroughFunctionality>();

            return true;
        }

        public void Shutdown()
        {
            
        }

        public void Step()
        {
            
        }
    }
}
