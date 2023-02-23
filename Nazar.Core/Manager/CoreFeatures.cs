using System;
using System.Collections.Generic;
using NazAR.Common;
using NazAR.Core.Manager.UI;
using NazAR.Core.Passthrough;
using StereoKit;
using StereoKit.Framework;

namespace NazAR.Core.Manager;

public class CoreFeatures : IStepper
{
    internal static List<Type> PermanentFeatures = new();
    internal static List<Type> ToggleableFeatures = new();
    internal static Dictionary<string, IStepper> ActiveToggleableFeatures = new();


    public CoreFeatures()
    {
        AddNewFeature(typeof(PassthroughExtension));
        SK.AddStepper<FeatureManagerUI>();

    }

    public bool Enabled => true;

    public bool Initialize()
    {
        return true;
    }


    public void Shutdown()
    {
    }

    public void Step()
    {
    }

    private void AddNewFeature(Type stepperType)
    {
        var passthroughExtension = (IUiStepper)SK.AddStepper(stepperType);
        PermanentFeatures.Add(stepperType);

        var passthroughInterface = passthroughExtension.GetUserInterface();
        if (passthroughInterface != null)
        {
            ToggleableFeatures.Add(passthroughInterface);
        }
    }
}