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
        private Pose demoSelectPose = new Pose();
        private Sprite powerButton;
        private List<string> activatedSteppers= new List<string>();

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
                    var featureClass = allSteppers.FirstOrDefault(el => el.Name == demoName);
                    if (activatedSteppers.Contains(demoName))
                    {
                        SK.RemoveStepper(featureClass);
                        activatedSteppers.Remove(demoName);
                    } else
                    {
                        SK.AddStepper(featureClass);
                        activatedSteppers.Add(demoName);
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
