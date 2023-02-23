﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nazar.SKit.Framework;
using StereoKit;
using StereoKit.Framework;

namespace Nazar.Extension.Features
{
    internal class ExtensionManager : IStepper
    {
        public bool Enabled { get; }

        internal List<Type> ToggleableFeatures = new();
        internal Dictionary<string, IStepper> ActiveToggleableFeatures = new();

        private Pose menuPose;
        private Sprite powerButton;

        public bool Initialize()
        {
            powerButton = Sprite.FromTex(Tex.FromFile("power.png"));

            menuPose.position = new Vec3(0, 0, -0.6f);
            menuPose.orientation = Quat.LookDir(-Vec3.Forward);
            return true;
        }

        public void Step()
        {

            // Make a window for demo selection
            UI.WindowBegin("NazarCore Extensions Settings", ref menuPose, new Vec2(50 * U.cm, 0));
            foreach (string demoName in ToggleableFeatures.Select(el => el.Name))
            {
                // If the button is pressed
                if (UI.Button(demoName))
                {
                    if (!ActiveToggleableFeatures.ContainsKey(demoName))
                    {
                        EnableFeature(demoName);
                    }
                    else
                    {
                        DisableFeature(demoName);
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

        public void Shutdown() { }


        public void AddNewFeature(Type stepperType)
        {
            ToggleableFeatures.Add(stepperType);
        }

        public void DisableFeature(string demoName)
        {
            SK.RemoveStepper(ActiveToggleableFeatures[demoName]);
            ActiveToggleableFeatures.Remove(demoName);
        }

        public void EnableFeature(string demoName)
        {
            Type featureType = ToggleableFeatures.FirstOrDefault(el => el.Name == demoName);
            ActiveToggleableFeatures[demoName] = (IStepper)SK.AddStepper(featureType);
        }
    }
}
