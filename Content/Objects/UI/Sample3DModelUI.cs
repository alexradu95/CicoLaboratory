using StereoKit;
using StereoKit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CicoLaboratory.Content.Objects.UI
{
    internal class Sample3DModelUI : IStepper
    {
        // Mixed Reality also provides us with the opportunity to turn objects into interfaces!
        // Instead of using the old ‘window’ paradigm, we can create 3D models and apply UI elements to their surface!
        // StereoKit uses ‘handles’ to accomplish this, a grabbable area that behaves much like a window,
        // but with a few more options for customizing layout and size.

        Model clipboard;
        Sprite logoSprite;
        Pose clipboardPose = new Pose(.4f, 1, 0, Quat.LookDir(-1, 0, 1));
        bool clipToggle;
        float clipSlider;
        int clipOption = 1;

        public bool Enabled => throw new NotImplementedException();

        public bool Initialize()
        {
            // We’ll load up a clipboard, so we can attach an interface to that!
            clipboard = Model.FromFile("Clipboard.glb", Default.ShaderUI);
            logoSprite = Sprite.FromFile("StereoKitWide.png", SpriteType.Single);

            return true;
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public void Step()
        {
            // And, similar to the window previously, here’s how you would turn it into a grabbable interface!
            // This behaves the same, except we’re defining where the grabbable region is specifically,
            // and then drawing our own model instead of a plain bar. You’ll also notice we’re drawing using an identity matrix.
            // This takes advantage of how HandleBegin pushes the handle’s pose onto the Hierarchy transform stack!
            StereoKit.UI.HandleBegin("Clip", ref clipboardPose, clipboard.Bounds);
            clipboard.Draw(Matrix.Identity);

            // Once we’ve done that, we also need to define the layout area of the model,
            // where UI elements will go.This is different for each model,
            // so you’ll need to plan this around the size of your object!
            StereoKit.UI.LayoutArea(new Vec3(12, 15, 0) * U.cm, new Vec2(24, 30) * U.cm);

            // Then after that? We can just add UI elements like normal!
            StereoKit.UI.Image(logoSprite, new Vec2(22, 0) * U.cm);

            StereoKit.UI.Toggle("Toggle", ref clipToggle);
            StereoKit.UI.HSlider("Slide", ref clipSlider, 0, 1, 0, 22 * U.cm);

            // And while we’re at it, here’s a quick example of doing a radio button group!
            // Not much ‘radio’ actually happening, but it’s still pretty simple.
            // Pair it with an enum, or an integer, and have fun!
            if (StereoKit.UI.Radio("Radio1", clipOption == 1)) clipOption = 1;
            StereoKit.UI.SameLine();
            if (StereoKit.UI.Radio("Radio2", clipOption == 2)) clipOption = 2;
            StereoKit.UI.SameLine();
            if (StereoKit.UI.Radio("Radio3", clipOption == 3)) clipOption = 3;

            // As with windows, Handles need an End call.
            StereoKit.UI.HandleEnd();
        }

    }
}
