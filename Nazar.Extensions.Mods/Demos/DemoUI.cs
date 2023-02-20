﻿/// :CodeDoc: Guides 2 User Interface
/// # Building UI in StereoKit
/// 
/// StereoKit uses an immediate mode UI system. Basically, you define the UI 
/// every single frame you want to see it! Sounds a little odd at first, but
/// it does have some pretty tremendous advantages. Since very little state
/// is actually stored, you can add, remove, and change your UI elements with
/// trivial and standard code structures! You'll find that you often have
/// much less UI code, with far fewer places for things to go wrong.
///
/// The goal for this UI API is to get you up and running as fast as possible
/// with a working UI! This does mean trading some design flexibility for API 
/// simplicity, but we also strive to retain configurability for those that
/// need a little extra.
/// 
/// :End:

using StereoKit;
using StereoKit.Framework;

namespace nazar.core.Features.Demos;

class DemoUI : IStepper
{
    /// :CodeDoc: Guides User Interface
    /// ## Making a Window
    /// 
    /// ![Simple UI]({{site.url}}/img/screenshots/GuideUserInterface.jpg)
    /// 
    /// Since StereoKit doesn't store state, you'll have to keep track of
    /// your data yourself! But that's actually a pretty good thing, since
    /// you'll probably do that one way or another anyhow. Here we've got a
    /// Pose for the window, off to the left and facing to the right, as well
    /// as a boolean for a toggle, and a float that we'll use as a slider!
    /// We'll add this code to our initialization section.
    Pose  windowPose = new Pose(-.4f, 0, 0, Quat.LookDir(1,0,1));
    public bool Enabled => throw new System.NotImplementedException();
    bool  showHeader = true;
    float slider     = 0.5f;

    Sprite powerSprite = Sprite.FromFile("power.png", SpriteType.Single);
    /// :End:

    Model  clipboard     = Model.FromFile("Clipboard.glb", Default.ShaderUI);
    Sprite logoSprite    = Sprite.FromFile("StereoKitWide.png", SpriteType.Single);
    Pose   clipboardPose = new Pose(.4f,0,0, Quat.LookDir(-1,0,1));
    bool   clipToggle;
    float  clipSlider;
    int    clipOption = 1;

    public void Step()
    {

        /// :CodeDoc: Guides User Interface
        /// Then we'll move over to the application step where we'll do the
        /// rest of the UI code!
        ///
        /// We'll start with a window titled "Window" that's 20cm wide, and
        /// auto-resizes on the y-axis. The U class is pretty helpful here,
        /// as it allows us to reason more visually about the units we're
        /// using! StereoKit uses meters as its base unit, which look a
        /// little awkward as raw floats, especially in the millimeter range.
        /// 
        /// We'll also use a toggle to turn the window's header on and off!
        /// The value from that toggle is passed in here via the showHeader
        /// field.
        /// 
        UI.WindowBegin("Window", ref windowPose, new Vec2(20, 0) * U.cm, showHeader?UIWin.Normal:UIWin.Body);
        ///
        /// When you begin a window, all visual elements are now relative to
        /// that window! UI takes advantage of the Hierarchy class and pushes
        /// the window's pose onto the Hierarchy stack. Ending the window
        /// will pop the pose off the hierarchy stack, and return things to
        /// normal!
        /// 
        /// Here's that toggle button! You'll also notice our use of 'ref'
        /// values in a lot of the UI code. UI functions typically follow the
        /// pattern of returning true/false to indicate they've been 
        /// interacted with during the frame, so you can nicely wrap them in
        /// 'if' statements to react to change!
        /// 
        /// Then with the 'ref' parameter, we let you pass in the current
        /// state of the UI element. The UI element will update that value
        /// for you based on user interaction, but you can also change it
        /// yourself whenever you want to!
        /// 
        UI.Toggle("Show Header", ref showHeader);
        /// 
        /// Here's an example slider! We start off with a label element, and
        /// tell the UI to keep the next item on the same line. The slider
        /// clamps to the range [0,1], and will step at intervals of 0.2. If
        /// you want it to slide continuously, you can just set the `step`
        /// value to 0!
        /// 
        UI.Label("Slide");
        UI.SameLine();
        UI.HSlider("slider", ref slider, 0, 1, 0.2f, 72 * U.mm);
        ///
        /// Here's how you use a simple button! Just check it with an 'if'.
        /// Any UI method will return true on the frame when their value or
        /// state has changed.
        /// 
        if (UI.ButtonImg("Exit", powerSprite))
            SK.Quit();
        /// 
        /// And for every begin, there must also be an end! StereoKit will
        /// log errors when this occurs, so keep your eyes peeled for that!
        /// 
        UI.WindowEnd();
        /// 
        /// ## Custom Windows
        /// 
        /// ![Simple UI]({{site.url}}/img/screenshots/GuideUserInterfaceCustom.jpg)
        /// 
        /// Mixed Reality also provides us with the opportunity to turn
        /// objects into interfaces! Instead of using the old 'window'
        /// paradigm, we can create 3D models and apply UI elements to their
        /// surface! StereoKit uses 'handles' to accomplish this, a grabbable
        /// area that behaves much like a window, but with a few more options
        /// for customizing layout and size.
        ///
        /// We'll load up a clipboard, so we can attach an interface to that!
        /// 
        /// ```csharp
        /// Model clipboard = Model.FromFile("Clipboard.glb");
        /// ```
        /// 
        /// And, similar to the window previously, here's how you would turn
        /// it into a grabbable interface! This behaves the same, except
        /// we're defining where the grabbable region is specifically, and
        /// then drawing our own model instead of a plain bar. You'll also
        /// notice we're drawing using an identity matrix. This takes
        /// advantage of how HandleBegin pushes the handle's pose onto the
        /// Hierarchy transform stack!
        /// 
        UI.HandleBegin("Clip", ref clipboardPose, clipboard.Bounds);
        clipboard.Draw(Matrix.Identity);
        ///
        /// Once we've done that, we also need to define the layout area of
        /// the model, where UI elements will go. This is different for each
        /// model, so you'll need to plan this around the size of your
        /// object!
        /// 
        UI.LayoutArea(new Vec3(12, 15, 0) * U.cm, new Vec2(24, 30) * U.cm);
        ///
        /// Then after that? We can just add UI elements like normal!
        /// 
        UI.Image(logoSprite, new Vec2(22,0) * U.cm);

        UI.Toggle("Toggle", ref clipToggle);
        UI.HSlider("Slide", ref clipSlider, 0, 1, 0, 22 * U.cm);
        ///
        /// And while we're at it, here's a quick example of doing a radio
        /// button group! Not much 'radio' actually happening, but it's still
        /// pretty simple. Pair it with an enum, or an integer, and have fun!
        ///
        if (UI.Radio("Radio1", clipOption == 1)) clipOption = 1;
        UI.SameLine();
        if (UI.Radio("Radio2", clipOption == 2)) clipOption = 2;
        UI.SameLine();
        if (UI.Radio("Radio3", clipOption == 3)) clipOption = 3;
        ///
        /// As with windows, Handles need an End call.
        /// 
        UI.HandleEnd();
        /// :End:

        // Just moving this UI out of the way so it doesn't show in
        // screenshots or anything.
        UI.PushSurface(new Pose(0,1000,0, Quat.Identity));

        /// :CodeDoc: Guides User Interface
        /// 
        /// ## An Important Note About IDs
        /// 
        /// StereoKit does store a small amount of information about the UI's
        /// state behind the scenes, like which elements are active and for
        /// how long. This internal data is attached to the UI elements via
        /// a combination of their own ids, and the parent Window/Handle's 
        /// id!
        /// 
        /// This means you should be careful to NOT re-use ids within a
        /// Window/Handle, otherwise you may find ghost interactions with
        /// elements that share the same ids. If you need to have elements
        /// with the same id, or if perhaps you don't know in advance that
        /// all your elements will certainly be unique, UI.PushId and
        /// UI.PopId can be used to mitigate the issue by using the same
        /// hierarchy id mixing that the Windows use to prevent collisions
        /// with the same ids in other Windows/Handles.
        /// 
        /// Here's the same set of radio options, but all of them have the 
        /// same name/id!
        ///
        UI.PushId(1);
        if (UI.Radio("Radio", clipOption == 1)) clipOption = 1;
        UI.PopId();

        UI.SameLine();
        UI.PushId(2);
        if (UI.Radio("Radio", clipOption == 2)) clipOption = 2;
        UI.PopId();

        UI.SameLine();
        UI.PushId(3);
        if (UI.Radio("Radio", clipOption == 3)) clipOption = 3;
        UI.PopId();
        /// :End:
		
        UI.PopSurface();

        /// :CodeDoc: Guides User Interface
        /// ## What's Next?
        /// 
        /// And there you go! That's how UI works in StereoKit, pretty 
        /// simple, huh? For further reference, and more UI methods, check 
        /// out the [UI class documentation]({{site.url}}/Pages/Reference/UI.html).
        /// 
        /// If you'd like to see the complete code for this sample, 
        /// [check it out on Github](https://github.com/StereoKit/StereoKit/blob/master/Examples/StereoKitTest/Demos/DemoUI.cs)!
        /// :End:


        /// :CodeSample: UI.VolumeAt
        /// This code will draw an axis at the index finger's location when
        /// the user pinches while inside a VolumeAt.
        /// 
        /// ![UI.InteractVolume]({{site.screen_url}}/InteractVolume.jpg)
        /// 
        // Draw a transparent volume so the user can see this space
        Vec3  volumeAt   = new Vec3(0,0.2f,-0.4f);
        float volumeSize = 0.2f;
        Default.MeshCube.Draw(Default.MaterialUIBox, Matrix.TS(volumeAt, volumeSize));

        BtnState volumeState = UI.VolumeAt("Volume", new Bounds(volumeAt, Vec3.One*volumeSize), UIConfirm.Pinch, out Handed hand);
        if (volumeState != BtnState.Inactive)
        {
            // If it just changed interaction state, make it jump in size
            float scale = volumeState.IsChanged()
                ? 0.1f
                : 0.05f;
            Lines.AddAxis(Input.Hand(hand)[FingerId.Index, JointId.Tip].Pose, scale);
        }
        /// :End:

    }

    public bool Initialize() {

        return true;

    }
    public void Shutdown() { }
}