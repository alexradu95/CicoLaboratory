using CicoLaboratory.Content.Objects.UI;
using SKTemplate_Maui.Content.Objects.Features;
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

    public void Init()
    {
        //SK.AddStepper<DebugHands>();
        //SK.AddStepper<Sample3DModelUI>();
        //SK.AddStepper<SampleUI>();
    }

    public void Step()
    {

    }
}