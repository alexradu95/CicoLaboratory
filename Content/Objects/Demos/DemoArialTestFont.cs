using StereoKit;
using StereoKit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CicoLaboratory.Content.Objects.Demos
{
    class DemoArialTestFont : IStepper
    {
        TextStyle style;

        public bool Enabled => throw new NotImplementedException();

        public bool Initialize()
        {
            style = Text.MakeStyle(
                Font.FromFile("C:/Windows/Fonts/Arial.ttf") ?? Default.Font,
                2 * U.cm,
                Color.White);

            return true;
        }
        public void Shutdown() { }
        public void Step()
        {
            Color32 col = new Color32(0, 255, 0, 255);

            Hierarchy.Push(Matrix.T(-0.06f, .05f, 0));
            Text.Add("Y Top", Matrix.TR(new Vec3(0, .04f, 0), Quat.LookDir(0, 0, 1)), style, TextAlign.TopCenter);
            Lines.Add(new Vec3(-0.05f, .04f, 0), new Vec3(.05f, .04f, 0), col, 0.001f);

            Text.Add("Y Center", Matrix.TR(new Vec3(0, .0f, 0), Quat.LookDir(0, 0, 1)), style, TextAlign.Center);
            Lines.Add(new Vec3(-0.05f, 0, 0), new Vec3(.05f, 0, 0), col, 0.001f);

            Text.Add("Y Bottom", Matrix.TR(new Vec3(0, -.04f, 0), Quat.LookDir(0, 0, 1)), style, TextAlign.BottomCenter);
            Lines.Add(new Vec3(-0.05f, -.04f, 0), new Vec3(.05f, -.04f, 0), col, 0.001f);
            Hierarchy.Pop();

            Hierarchy.Push(Matrix.T(0.02f, 0.05f, 0));
            Text.Add("2cm Tall", Matrix.R(Quat.LookDir(0, 0, 1)), style, TextAlign.CenterLeft);

            Lines.Add(new Vec3(0, -.01f, 0), new Vec3(.11f, -.01f, 0), col, 0.001f);
            Lines.Add(new Vec3(0, .01f, 0), new Vec3(.11f, .01f, 0), col, 0.001f);
            Hierarchy.Pop();

            Hierarchy.Push(Matrix.T(0, -0.06f, 0));
            Text.Add("X Left", Matrix.TR(new Vec3(0, 0.04f, 0), Quat.LookDir(0, 0, 1)), style, TextAlign.CenterLeft);
            Text.Add("X Center", Matrix.TR(new Vec3(0, 0, 0), Quat.LookDir(0, 0, 1)), style, TextAlign.Center);
            Text.Add("X Right", Matrix.TR(new Vec3(0, -0.04f, 0), Quat.LookDir(0, 0, 1)), style, TextAlign.CenterRight);
            Lines.Add(new Vec3(0, .06f, 0), new Vec3(0, -.06f, 0), col, 0.001f);
            Hierarchy.Pop();

        }
    }
}
