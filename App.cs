using SKTemplate_Maui.Content.Objects.Features;
using SKTemplate_Maui.Content.Objects.UI;
using StereoKit;

namespace CicoLaboratory;

public class App
{
    public SKSettings Settings => new SKSettings
    {
        appName = "SKTemplate_Maui",
        assetsFolder = "Assets",
        displayPreference = DisplayMode.MixedReality
    };

    //SampleUI sampleUI = new SampleUI();
    Sample3DModelUI sample3DModel = new Sample3DModelUI();
    DebugHands debugHands = new DebugHands();

    public void Init()
    {
        sample3DModel.Init();
        debugHands.Init();
    }

    public void Step()
    {
        //sampleUI.Step();
        sample3DModel.Step();
        debugHands.Step();
    }
}