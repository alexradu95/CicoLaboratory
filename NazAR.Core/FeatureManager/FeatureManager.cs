using NazAR.Core.FeatureManager.UI;
using NazAR.Core.Passthrough;
using StereoKit;
using StereoKit.Framework;
using System;
using System.Collections.Generic;

namespace NazAR.Core.FeatureManager;

public class FeatureManager : IStepper
{
    internal static List<Type> PermanentFeatures = new();
    internal static List<Type> ToggleableFeatures = new();
    internal static Dictionary<string, IStepper> ActiveToggleableFeatures = new();


    public FeatureManager()
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
        var passthroughExtension = (INazarStepper) SK.AddStepper(stepperType);
        PermanentFeatures.Add(stepperType);

        var passthroughInterface = passthroughExtension.GetUserInterface();
        if (passthroughInterface != null)
        {
            ToggleableFeatures.Add(passthroughInterface);
        }
    }
}