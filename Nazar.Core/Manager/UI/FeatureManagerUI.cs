using System;
using System.Linq;
using StereoKit;
using StereoKit.Framework;

namespace NazAR.Core.Manager.UI;

internal class FeatureManagerUI : IStepper
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
        // Make a window for demo selection
        StereoKit.UI.WindowBegin("Core Mods Settings", ref menuPose, new Vec2(50 * U.cm, 0));
        foreach (string demoName in CoreFeatures.ToggleableFeatures.Select(el => el.Name))
        {
            // If the button is pressed
            if (StereoKit.UI.Button(demoName))
            {
                if (!CoreFeatures.ActiveToggleableFeatures.ContainsKey(demoName))
                {
                    ActivateFeature(demoName);
                }
                else
                {
                    RemoveFeature(demoName);
                }
            }

            StereoKit.UI.SameLine();
        }

        StereoKit.UI.NextLine();
        StereoKit.UI.HSeparator();
        if (StereoKit.UI.ButtonImg("Exit", powerButton))
            SK.Quit();
        StereoKit.UI.WindowEnd();
    }

    private static void RemoveFeature(string demoName)
    {
        SK.RemoveStepper(CoreFeatures.ActiveToggleableFeatures[demoName]);
        CoreFeatures.ActiveToggleableFeatures.Remove(demoName);
    }

    private static void ActivateFeature(string demoName)
    {
        Type featureType = CoreFeatures.ToggleableFeatures.FirstOrDefault(el => el.Name == demoName);
        CoreFeatures.ActiveToggleableFeatures[demoName] = (IStepper)SK.AddStepper(featureType);
    }
}