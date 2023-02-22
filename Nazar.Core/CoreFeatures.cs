using NazAR.Core.Passthrough;
using StereoKit;
using StereoKit.Framework;
using System;
using System.Collections.Generic;

namespace Nazar.Core;

public class CoreFeatures : IStepper
{
    internal static List<Type> ToggleableFeatures = new();
    internal static Dictionary<string, IStepper> ActiveToggleableFeatures = new();


    public CoreFeatures()
    {
        SK.AddStepper<PassthroughExtension>();
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

    private void AddNewFeature(Type typeofClass)
    {

    }
}