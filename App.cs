using CicoLaboratory.Content;
using CicoLaboratory.Content.Environment;
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
        SK.AddStepper<FeatureActivatorMenu>();
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
        //SK.AddStepper<DemoText>();
        //SK.AddStepper<DemoTextInput>();
        //SK.AddStepper<DemoTextures>();
        //SK.AddStepper<DemoUI>();
        //SK.AddStepper<DemoUISettings>();
        //SK.AddStepper<DemoUnicode>();
        //SK.AddStepper<DemoWelcome>();
        //SK.AddStepper<DemoWorldMesh>();
        //SK.AddStepper<ControllerLocomotion>();
        //SK.AddStepper<LaboratoryEnvironment>();
    }

    public void Step()
    {

    }
}