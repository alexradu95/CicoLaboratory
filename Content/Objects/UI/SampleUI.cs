using StereoKit;

namespace SKTemplate_Maui.Content.Objects.UI
{
    internal class SampleUI
    {
        // Since StereoKit doesn’t store state, we will have to track data ourselves!

        // Got a Pose for the window, off to the left and facing to the right
        Pose windowPose = new Pose(-.4f, 0, 0, Quat.LookDir(1, 0, 1));

        // Boolean for the toggle header
        bool showHeader = true;

        // A floating value that will be mapped to the slider
        float slider = 0.5f;

        public void Init()
        {

        }

        public void Step()
        {
            // We’ll start with a window titled “Window” that’s 20cm wide, and auto-resizes on the y-axis.
            // The U class is pretty helpful here, as it allows us to reason more visually about the units we’re using!
            // StereoKit uses meters as its base unit, which look a little awkward as raw floats, especially in the millimeter range.
            StereoKit.UI.WindowBegin("Window", ref windowPose, new Vec2(20, 0) * U.cm, showHeader ? UIWin.Normal : UIWin.Body);

            // We’ll also use a toggle to turn the window’s header on and off! The value from that toggle is passed in here via the showHeader field.
            // You’ll also notice our use of ‘ref’ values in a lot of the UI code.
            // UI functions typically follow the pattern of returning true / false to indicate they’ve been interacted with during
            // the frame, so you can nicely wrap them in ‘if’ statements to react to change!
            // Then with the ‘ref’ parameter, we let you pass in the current state of the UI element.
            // The UI element will update that value for you based on user interaction, but you can also change it yourself whenever you want to!
            StereoKit.UI.Toggle("Show Header", ref showHeader);

            // When you begin a window, all visual elements are now relative to that window! 
            // UI takes advantage of the Hierarchy class and pushes the window’s pose onto the Hierarchy stack. 
            // Ending the window will pop the pose off the hierarchy stack, and return things to normal!

            // Here’s an example slider! We start off with a label element
            // Tell the UI to keep the next item on the same line.
            // The slider clamps to the range[0, 1], and will step at intervals of 0.2.
            // If you want it to slide continuously, you can just set the step value to 0!
            StereoKit.UI.Label("Slide");
            StereoKit.UI.SameLine();
            StereoKit.UI.HSlider("slider", value: ref slider, min: 0, max: 1, step: 0.2f, width: 72 * U.mm);

            // Here’s how you use a simple button!
            // Just check it with an ‘if’.
            // Any UI method will return true on the frame when their value or state has changed.
            if (StereoKit.UI.Button("Exit"))
                SK.Quit();

            // And for every begin, there must also be an end!
            // StereoKit will log errors when this occurs, so keep your eyes peeled for that!
            StereoKit.UI.WindowEnd();
        }
    }
}
