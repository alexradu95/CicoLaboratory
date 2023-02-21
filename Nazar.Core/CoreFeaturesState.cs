using System;
using System.Collections.Generic;
using Nazar.Core.Passthrough;
using StereoKit.Framework;

namespace Nazar.Core;

internal class CoreFeaturesState
{
    internal static List<Type> ToggleableFeatures = new();
    internal static Dictionary<string, IStepper> ActiveToggleableFeatures = new();

    #region Permanent features

    internal static PassthroughExtension PassthroughExtension;
    internal static CoreFeaturesMenu CoreFeatures;

    #endregion
}