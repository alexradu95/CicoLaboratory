using StereoKit.Framework;
using StereoKit;

namespace SKTemplate_Maui.Content.Objects.Features
{
    internal class DebugHands
    {
        static Pose optionsPose = new Pose(0.5f, 0, -0.5f, Quat.LookDir(-1, 0, 1));
        bool showHands = true;
        bool showJoints = false;
        bool showAxes = true;
        bool showPointers = true;
        bool showHandMenus = true;
        bool showHandSize = true;

        Mesh jointMesh;
        HandMenuRadial handMenu;

        public void Init()
        {
            jointMesh = Mesh.GenerateSphere(1);

            /// Since hands are so central to interaction, accessing hand information needs
            /// to be really easy to get! So here's how you might find the fingertip of the right
            /// hand! If you ignore IsTracked, this'll give you the last known position for that
            /// finger joint.
            Hand hand = Input.Hand(Handed.Right);
            if (hand.IsTracked)
            {
                Vec3 fingertip = hand[FingerId.Index, JointId.Tip].position;
            }
            /// Pretty straightforward! And if you prefer calling a function instead of using the
            /// [] operator, that's cool too! You can call `hand.Get(FingerId.Index, JointId.Tip)`
            /// instead!
            /// 
            /// If that's too granular for you, there's easy ways to check for pinching and 
            /// gripping! Pinched will tell you if a pinch is currently happening, JustPinched
            /// will tell you if it just started being pinched this frame, and JustUnpinched will
            /// tell you if the pinch just stopped this frame!
            if (hand.IsPinched) { }
            if (hand.IsJustPinched) { }
            if (hand.IsJustUnpinched) { }

            if (hand.IsGripped) { }
            if (hand.IsJustGripped) { }
            if (hand.IsJustUngripped) { }
            /// These are all convenience functions wrapping the `hand.pinchState` bit-flag, so you
            /// can also use that directly if you want to do some bit-flag wizardry!
            /// :End:

            /// :CodeSample: HandMenuRadial HandRadialLayer HandMenuItem
            /// ### Basic layered hand menu
            /// 
            /// The HandMenuRadial is an `IStepper`, so it should be registered with 
            /// `StereoKitApp.AddStepper` so it can run by itself! It's recommended to
            /// keep track of it anyway, so you can remove it when you're done with it
            /// via `StereoKitApp.RemoveStepper`
            /// 
            /// The constructor uses a params style argument list that makes it easy and
            /// clean to provide lists of items! This means you can assemble the whole
            /// menu on a single 'line'. You can still pass arrays instead if you prefer
            /// that!
            handMenu = SK.AddStepper(new HandMenuRadial(
                new HandRadialLayer("Root",
                    new HandMenuItem("File", null, null, "File"),
                    new HandMenuItem("Edit", null, null, "Edit"),
                    new HandMenuItem("About", null, () => Log.Info(SK.VersionName)),
                    new HandMenuItem("Cancel", null, null)),
                new HandRadialLayer("File",
                    new HandMenuItem("New", null, () => Log.Info("New")),
                    new HandMenuItem("Open", null, () => Log.Info("Open")),
                    new HandMenuItem("Close", null, () => Log.Info("Close")),
                    new HandMenuItem("Back", null, null, HandMenuAction.Back)),
                new HandRadialLayer("Edit",
                    new HandMenuItem("Copy", null, () => Log.Info("Copy")),
                    new HandMenuItem("Paste", null, () => Log.Info("Paste")),
                    new HandMenuItem("Back", null, null, HandMenuAction.Back))));
            /// :End:
        }

        public void Step()
        {
            StereoKit.UI.WindowBegin("Options", ref optionsPose, new Vec2(24, 0) * U.cm);
            StereoKit.UI.Label("Show");
            if (StereoKit.UI.Toggle("Hands", ref showHands))
                Input.HandVisible(Handed.Max, showHands);
            StereoKit.UI.SameLine();
            StereoKit.UI.Toggle("Joints", ref showJoints);
            StereoKit.UI.SameLine();
            StereoKit.UI.Toggle("Axes", ref showAxes);
            StereoKit.UI.SameLine();
            StereoKit.UI.Toggle("Hand Size", ref showHandSize);
            StereoKit.UI.SameLine();
            StereoKit.UI.Toggle("Pointers", ref showPointers);
            StereoKit.UI.SameLine();
            StereoKit.UI.Toggle("Menu", ref showHandMenus);
            StereoKit.UI.Label("Color");
            if (StereoKit.UI.Button("Rainbow"))
                ColorizeFingers(16,
                    new Gradient(
                        new GradientKey(Color.HSV(0.0f, 1, 1), 0.1f),
                        new GradientKey(Color.HSV(0.2f, 1, 1), 0.3f),
                        new GradientKey(Color.HSV(0.4f, 1, 1), 0.5f),
                        new GradientKey(Color.HSV(0.6f, 1, 1), 0.7f),
                        new GradientKey(Color.HSV(0.8f, 1, 1), 0.9f)),
                    new Gradient(
                        new GradientKey(new Color(1, 1, 1, 0), 0),
                        new GradientKey(new Color(1, 1, 1, 0), 0.4f),
                        new GradientKey(new Color(1, 1, 1, 1), 0.9f)));
            StereoKit.UI.SameLine();
            if (StereoKit.UI.Button("Black"))
                ColorizeFingers(16,
                    new Gradient(new GradientKey(new Color(0, 0, 0, 1), 1)),
                    new Gradient(
                        new GradientKey(new Color(1, 1, 1, 0), 0),
                        new GradientKey(new Color(1, 1, 1, 0), 0.4f),
                        new GradientKey(new Color(1, 1, 1, 1), 0.6f),
                        new GradientKey(new Color(1, 1, 1, 1), 0.9f)));
            StereoKit.UI.SameLine();
            if (StereoKit.UI.Button("Full Black"))
                ColorizeFingers(16,
                    new Gradient(new GradientKey(new Color(0, 0, 0, 1), 1)),
                    new Gradient(
                        new GradientKey(new Color(1, 1, 1, 0), 0),
                        new GradientKey(new Color(1, 1, 1, 1), 0.05f),
                        new GradientKey(new Color(1, 1, 1, 1), 1.0f)));
            StereoKit.UI.SameLine();
            if (StereoKit.UI.Button("Normal"))
                ColorizeFingers(16,
                    new Gradient(new GradientKey(new Color(1, 1, 1, 1), 1)),
                    new Gradient(
                        new GradientKey(new Color(.4f, .4f, .4f, 0), 0),
                        new GradientKey(new Color(.6f, .6f, .6f, 0), 0.4f),
                        new GradientKey(new Color(.8f, .8f, .8f, 1), 0.55f),
                        new GradientKey(new Color(1, 1, 1, 1), 1)));
            StereoKit.UI.WindowEnd();

            if (showJoints) DrawJoints(jointMesh, Default.Material);
            if (showAxes) DrawAxes();
            if (showPointers) DrawPointers();
            if (showHandSize) DrawHandSize();
            if (showHandMenus)
            {
                DrawHandMenu(Handed.Right);
                DrawHandMenu(Handed.Left);
            }
        }

        public void Shutdown()
        {
            /// :CodeSample: HandMenuRadial HandRadialLayer HandMenuItem
            SK.RemoveStepper(handMenu);
            /// :End:
        }

        private void ColorizeFingers(int size, Gradient horizontal, Gradient vertical)
        {
            Tex tex = new Tex(TexType.Image, TexFormat.Rgba32Linear);
            tex.AddressMode = TexAddress.Clamp;

            Color32[] pixels = new Color32[size * size];
            for (int y = 0; y < size; y++)
            {
                Color v = vertical.Get(1 - y / (size - 1.0f));
                for (int x = 0; x < size; x++)
                {
                    Color h = horizontal.Get(x / (size - 1.0f));
                    pixels[x + y * size] = v * h;
                }
            }
            tex.SetColors(size, size, pixels);

            Default.MaterialHand[MatParamName.DiffuseTex] = tex;
        }

        /// :CodeDoc: Guides Using Hands
        /// ## Hand Menu
        /// 
        /// Lets imagine you want to make a hand menu, you might need to know 
        /// if the user is looking at the palm of their hand! Here's a quick 
        /// example of using the palm's pose and the dot product to determine 
        /// this.
        static bool HandFacingHead(Handed handed)
        {
            Hand hand = Input.Hand(handed);
            if (!hand.IsTracked)
                return false;

            Vec3 palmDirection = (hand.palm.Forward).Normalized;
            Vec3 directionToHead = (Input.Head.position - hand.palm.position).Normalized;

            return Vec3.Dot(palmDirection, directionToHead) > 0.5f;
        }
        /// Once you have that information, it's simply a matter of placing a
        /// window off to the side of the hand! The palm pose Right direction
        /// points to different sides of each hand, so a different X offset
        /// is required for each hand.
        public static void DrawHandMenu(Handed handed)
        {
            if (!HandFacingHead(handed))
                return;

            // Decide the size and offset of the menu
            Vec2 size = new Vec2(4, 16);
            float offset = handed == Handed.Left ? -2 - size.x : 2 + size.x;

            // Position the menu relative to the side of the hand
            Hand hand = Input.Hand(handed);
            Vec3 at = hand[FingerId.Little, JointId.KnuckleMajor].position;
            Vec3 down = hand[FingerId.Little, JointId.Root].position;
            Vec3 across = hand[FingerId.Index, JointId.KnuckleMajor].position;

            Pose menuPose = new Pose(
                at,
                Quat.LookAt(at, across, at - down) * Quat.FromAngles(0, handed == Handed.Left ? 90 : -90, 0));
            menuPose.position += menuPose.Right * offset * U.cm;
            menuPose.position += menuPose.Up * (size.y / 2) * U.cm;

            // And make a menu!
            StereoKit.UI.WindowBegin("HandMenu", ref menuPose, size * U.cm, UIWin.Empty);
            StereoKit.UI.Button("Test");
            StereoKit.UI.Button("That");
            StereoKit.UI.Button("Hand");
            StereoKit.UI.WindowEnd();
        }
        /// :End:

        public static void DrawAxes()
        {
            for (int i = 0; i < (int)Handed.Max; i++)
            {
                Hand hand = Input.Hand((Handed)i);
                if (!hand.IsTracked)
                    continue;

                for (int finger = 0; finger < 5; finger++)
                {
                    for (int joint = 0; joint < 5; joint++)
                    {
                        Lines.AddAxis(hand[finger, joint].Pose);
                    }
                }
                Lines.AddAxis(hand.palm);
            }
        }

        public static void DrawJoints(Mesh jointMesh, Material jointMaterial)
        {
            for (int i = 0; i < (int)Handed.Max; i++)
            {
                Hand hand = Input.Hand((Handed)i);
                if (!hand.IsTracked)
                    continue;

                for (int finger = 0; finger < 5; finger++)
                {
                    for (int joint = 0; joint < 5; joint++)
                    {
                        HandJoint current = hand[finger, joint];
                        jointMesh.Draw(jointMaterial, Matrix.TRS(current.position, current.orientation, current.radius / 2));
                    }
                }
            }
        }

        /// :CodeDoc: Guides Using Hands
        /// ## Pointers
        /// 
        /// And lastly, StereoKit also has a pointer system! This applies to
        /// more than just hands. Head, mouse, and other devices will also
        /// create pointers into the scene. You can filter pointers based on
        /// source family and device capabilities, so this is a great way to 
        /// abstract a few more input sources nicely!
        public static void DrawPointers()
        {
            int hands = Input.PointerCount(InputSource.Hand);
            for (int i = 0; i < hands; i++)
            {
                Pointer pointer = Input.Pointer(i, InputSource.Hand);
                Lines.Add(pointer.ray, 0.5f, Color.White, Units.mm2m);
                Lines.AddAxis(pointer.Pose);
            }
        }
        /// :End:

        public static void DrawHandSize()
        {
            for (int h = 0; h < (int)Handed.Max; h++)
            {
                Hand hand = Input.Hand((Handed)h);
                if (!hand.IsTracked)
                    continue;

                HandJoint at = hand[FingerId.Middle, JointId.Tip];
                Vec3 pos = at.position + at.Pose.Forward * at.radius;
                Quat rot = at.orientation * Quat.FromAngles(-90, 0, 0);
                if (!HandFacingHead((Handed)h)) rot = rot * Quat.FromAngles(0, 180, 0);

                StereoKit.Text.Add(
                    (hand.size * 100).ToString(".0") + "cm",
                    Matrix.TRS(pos, rot, 0.3f),
                    TextAlign.XCenter | TextAlign.YBottom);
            }
        }
    }
}
