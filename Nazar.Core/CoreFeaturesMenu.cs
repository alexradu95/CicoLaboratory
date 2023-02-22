using System;
using System.Linq;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Core;

internal class CoreFeaturesMenu : IStepper
{
    private Pose menuPose;
    private Sprite powerButton;

    public bool Enabled => true;

    public bool Initialize()
    {
        powerButton = Sprite.FromTex(Tex.FromFile("power.png"));

        menuPose.position = new Vec3(0, 0, -0.6f);
        menuPose.orientation = Quat.LookDir(-Vec3.Forward);

        return true;
    }

    public void Shutdown()
    {
    }

    public void Step()
    {
        DrawMenu();
    }

    private void DrawMenu()
    {
        //// Make a window for demo selection
        //UI.WindowBegin("Core Mods Settings", ref menuPose, new Vec2(50 * U.cm, 0));
        //foreach (string demoName in FeaturesState.ToggleableFeatures.Select(el => el.Name))
        //{
        //    // If the button is pressed
        //    if (UI.Button(demoName))
        //    {
        //        if (!FeaturesState.ActiveToggleableFeatures.ContainsKey(demoName))
        //        {
        //            ActivateFeature(demoName);
        //        }
        //        else
        //        {
        //            RemoveFeature(demoName);
        //        }
        //    }

        //    UI.SameLine();
        //}

        //UI.NextLine();
        //UI.HSeparator();
        //if (UI.ButtonImg("Exit", powerButton))
        //    SK.Quit();
        //UI.WindowEnd();
    }

    private static void RemoveFeature(string demoName)
    {
        //SK.RemoveStepper(FeaturesState.ActiveToggleableFeatures[demoName]);
        //FeaturesState.ActiveToggleableFeatures.Remove(demoName);
    }

    private static void ActivateFeature(string demoName)
    {
        //Type featureType = FeaturesState.ToggleableFeatures.FirstOrDefault(el => el.Name == demoName);
        //FeaturesState.ActiveToggleableFeatures[demoName] = (IStepper) SK.AddStepper(featureType);
    }
}