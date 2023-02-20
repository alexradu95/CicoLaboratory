using Nazar.Core.Mods;
using StereoKit;
using StereoKit.Framework;
using System;
using System.Linq;

namespace Nazar.Core.Features
{
    internal class CoreFeaturesMenu : IStepper
    {

        private Pose demoSelectPose = new Pose();
        private Sprite powerButton;

        public bool Enabled => true;

        public bool Initialize()
        {
            powerButton = Sprite.FromTex(Tex.FromFile("power.png"));

            demoSelectPose.position = new Vec3(0, 0, -0.6f);
            demoSelectPose.orientation = Quat.LookDir(-Vec3.Forward);

            return true;
        }

        public void Shutdown() { }

        public void Step()
        {
            // Make a window for demo selection
            UI.WindowBegin("Core Mods Settings", ref demoSelectPose, new Vec2(50 * U.cm, 0));
            foreach (string demoName in CoreFeaturesState.AllFeatures.Select(el => el.Name))
            {
                // If the button is pressed
                if (UI.Button(demoName))
                {
                    if (!CoreFeaturesState.ActiveToggleableFeatures.ContainsKey(demoName))
                    {
                        Type featureType = CoreFeaturesState.AllFeatures.FirstOrDefault(el => el.Name == demoName);
                        CoreFeaturesState.ActiveToggleableFeatures[demoName] = (IStepper)SK.AddStepper(featureType);
                    }
                    else
                    {
                        SK.RemoveStepper(CoreFeaturesState.ActiveToggleableFeatures[demoName]);
                        CoreFeaturesState.ActiveToggleableFeatures.Remove(demoName);
                    }
                }
                UI.SameLine();
            }
            UI.NextLine();
            UI.HSeparator();
            if (UI.ButtonImg("Exit", powerButton))
                SK.Quit();
            UI.WindowEnd();
        }
    }
}
