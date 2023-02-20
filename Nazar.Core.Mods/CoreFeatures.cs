using Nazar.Core.Features;
using Nazar.Core.Features.Passthrough;
using StereoKit;
using StereoKit.Framework;
using System;
using System.Collections.Generic;

namespace Nazar.Core.Mods
{
    public class CoreFeatures : IStepper
    {
        public bool Enabled => true;

        public CoreFeatures()
        {
            // We instantiate it in the constructor because the PassthroughService must be initialized before SK
            CoreFeaturesState.ActivePermanentFeatures.Add(typeof(Passthrough).Name, SK.AddStepper<Passthrough>());
            CoreFeaturesState.ActivePermanentFeatures.Add(typeof(CoreFeaturesMenu).Name, SK.AddStepper<CoreFeaturesMenu>());
        }

        public bool Initialize() {

            CoreFeaturesState.AllFeatures.Add(typeof(PassthroughMenu));
            return true;

        }
       

        public void Shutdown() { }
        public void Step() { }
    }
}
