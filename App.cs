using CicoLaboratory.Content.Objects.UI;
using SKTemplate_Maui.Content.Objects.Features;
using StereoKit;
using StereoKitTest;
using StereoKitTest.Demos;

namespace CicoLaboratory;

public class App
{
    public SKSettings Settings => new SKSettings
    {
        appName = "SKTemplate_Maui",
        assetsFolder = "Assets",
        displayPreference = DisplayMode.MixedReality
    };

    public void Init()
    {
        //SK.AddStepper<DebugHands>();
        //SK.AddStepper<Sample3DModelUI>();
        //SK.AddStepper<SampleUI>();
        //SK.AddStepper<DemoControllers>();
        //SK.AddStepper<DemoGeometry>();
        //SK.AddStepper<DemoHands>();
        //SK.AddStepper<DemoLineRender>();
        //SK.AddStepper<DemoLines>();
        //SK.AddStepper<DemoManyObjects>();
        //SK.AddStepper<DemoMaterial>();
        //SK.AddStepper<DemoMath>();
        //SK.AddStepper<DemoNodes>();
        //SK.AddStepper<DemoPhysics>();
        //SK.AddStepper<DemoFilePicker>();
        //SK.AddStepper<DemoPointCloud>();
        //SK.AddStepper<DemoRayMesh>();
        //SK.AddStepper<DemoSkyLight>();
        SK.AddStepper<DemoText>();
    }

    public void Step()
    {

    }
}