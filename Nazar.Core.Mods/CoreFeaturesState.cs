using StereoKit.Framework;
using System;
using System.Collections.Generic;

namespace Nazar.Core.Features
{
    internal class CoreFeaturesState
    {
        internal static List<Type> AllFeatures = new List<Type>();
        internal static Dictionary<string, IStepper> ActivePermanentFeatures = new();
        internal static Dictionary<string, IStepper> ActiveToggleableFeatures = new();
    }
}
