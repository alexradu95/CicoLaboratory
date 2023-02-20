﻿using System;
using System.Collections.Generic;
using System.Linq;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Core.Mods
{
    public class CoreMods : IStepper
    {
        private List<Type> allSteppers = new List<Type>();

        Dictionary<string, IStepper> activeFeatures = new();

        private Pose demoSelectPose = new Pose();
        private Sprite powerButton;

        public bool Enabled => true;

        public CoreMods()
        {
            InitializePassthrough();
        }

        public bool Initialize()
        {
            InitializeUI();
            return true;
        }

        private void InitializePassthrough()
        {
        }

        private void InitializeUI()
        {
            powerButton = Sprite.FromTex(Tex.FromFile("power.png"));

            demoSelectPose.position = new Vec3(0, 0, -0.6f);
            demoSelectPose.orientation = Quat.LookDir(-Vec3.Forward);
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public void Step()
        {
            // Make a window for demo selection
            UI.WindowBegin("Core Mods Settings", ref demoSelectPose, new Vec2(50 * U.cm, 0));
            foreach (string demoName in allSteppers.Select(el => el.Name))
            {
                // If the button is pressed
                if (UI.Button(demoName))
                {
                    if (!activeFeatures.ContainsKey(demoName))
                    {
                        Type featureType = allSteppers.FirstOrDefault(el => el.Name == demoName);
                        activeFeatures[demoName] = (IStepper) SK.AddStepper(featureType);
                    }
                    else
                    {
                        SK.RemoveStepper(activeFeatures[demoName]);
                        activeFeatures.Remove(demoName);
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