using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using Java.Lang;
using StereoKit;
using System;
using System.Threading.Tasks;

namespace NazAR
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, Exported = true)]
    [IntentFilter(new[] { Intent.ActionMain }, Categories = new[] { "org.khronos.openxr.intent.category.IMMERSIVE_HMD", "com.oculus.intent.category.VR", Intent.CategoryLauncher })]
    public class MainActivity : AppCompatActivity, ISurfaceHolderCallback2
    {
        Launcher launcher;
        View surface;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            JavaSystem.LoadLibrary("openxr_loader");
            JavaSystem.LoadLibrary("StereoKitC");

            // Set up a surface for StereoKit to draw on
            Window.TakeSurface(this);
            Window.SetFormat(Format.Unknown);
            surface = new(this);
            SetContentView(surface);
            surface.RequestFocus();

            base.OnCreate(savedInstanceState);
            Microsoft.Maui.ApplicationModel.Platform.Init(this, savedInstanceState);

            Run(Handle);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Microsoft.Maui.ApplicationModel.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        static bool running = false;
        void Run(IntPtr activityHandle)
        {
            if (running)
                return;
            running = true;

            Task.Run(() =>
            {
                // If the launcher has a constructor that takes a string array, then
                // we'll use that, and pass the command line arguments into it on
                // creation
                Type appType = typeof(Launcher);
                launcher = appType.GetConstructor(new Type[] { typeof(string[]) }) != null
                    ? (Launcher)Activator.CreateInstance(appType, new object[] { new string[0] { } })
                    : (Launcher)Activator.CreateInstance(appType);
                if (launcher == null)
                    throw new System.Exception("StereoKit loader couldn't construct an instance of the Launcher!");

                // Initialize StereoKit, and the launcher
                SKSettings settings = launcher.Settings;
                settings.androidActivity = activityHandle;
                if (!SK.Initialize(settings))
                    return;
                launcher.Initialize();

                // Now loop until finished, and then shut down
                SK.Run(launcher.Step);

                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            });
        }

        // Events related to surface state changes
        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height) => SK.SetWindow(holder.Surface.Handle);
        public void SurfaceCreated(ISurfaceHolder holder) => SK.SetWindow(holder.Surface.Handle);
        public void SurfaceDestroyed(ISurfaceHolder holder) => SK.SetWindow(IntPtr.Zero);
        public void SurfaceRedrawNeeded(ISurfaceHolder holder) { }
    }
}