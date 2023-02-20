using Nazar.Core.Features.Passthrough;
using StereoKit.Framework;
using System;
using System.Collections.Generic;

namespace Nazar.Core.Features
{
    internal class CoreFeaturesState
    {
        #region Permanent features
        internal static PassthroughExtension PassthroughExtension;
        internal static CoreFeaturesMenu CoreFeatures;

        #endregion
        internal static List<Type> ToggleableFeatures = new List<Type>();
        internal static Dictionary<string, IStepper> ActiveToggleableFeatures = new();


    }
}
