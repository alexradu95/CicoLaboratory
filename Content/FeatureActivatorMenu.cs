using StereoKit;
using StereoKit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CicoLaboratory.Content
{
    internal class FeatureActivatorMenu : IStepper
    {


        private List<Type> allSteppers = new List<Type>();
        Dictionary<string, IStepper> activeFeatures = new();

        private Pose demoSelectPose = new Pose();
        private Sprite powerButton;

        public bool Enabled => true;

        public bool Initialize()
        {
            InitializeUI();
            FindDemoClasses();
            return true;
        }

        private void InitializeUI()
        {
            powerButton = Sprite.FromTex(Tex.FromFile("power.png"));

            demoSelectPose.position = new Vec3(0, 0, -0.6f);
            demoSelectPose.orientation = Quat.LookDir(-Vec3.Forward);
        }

        private void FindDemoClasses()
        {
            allSteppers = Assembly.GetExecutingAssembly().GetTypes().Where(a => a != typeof(IStepper) && typeof(IStepper).IsAssignableFrom(a)).ToList();
            allSteppers.Remove(typeof(FeatureActivatorMenu));
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public void Step()
        {
            // Make a window for demo selection
            UI.WindowBegin("Demos", ref demoSelectPose, new Vec2(50 * U.cm, 0));
            foreach(string demoName in allSteppers.Select(el => el.Name)) {
                if (UI.Button(demoName))
                {
                    if (activeFeatures.ContainsKey(demoName))
                    {
                        SK.RemoveStepper(activeFeatures[demoName]);
                        activeFeatures.Remove(demoName);
                    }
                    else
                    {
                        Type feature = allSteppers.FirstOrDefault(el => el.Name == demoName);
                        activeFeatures[demoName] = (IStepper) SK.AddStepper(feature);
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
