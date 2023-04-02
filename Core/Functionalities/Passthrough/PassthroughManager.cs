using Core.Functionalities.Passthrough.OpenXRBindings;
using Core.Functionalities.Passthrough.OpenXRBindings.Enums;
using Core.Functionalities.Passthrough.OpenXRBindings.Structs;
using Framework;
using StereoKit;

namespace Core.Functionalities.Passthrough;

public class PassthroughManager : Node
{
    private XrPassthroughLayerFB activeLayer;
    private XrPassthroughFB activePassthrough;
    private bool enabledPassthrough;
    private PassthroughOpenXRBindings openXRBindings;
    private Color oldColor;
    private bool oldSky;
    private bool passthroughRunning;

    public PassthroughManager()
    {
        if (SK.IsInitialized)
            Log.Err("PassthroughMod must be constructed before StereoKit is initialized!");
        Backend.OpenXR.RequestExt("XR_FB_passthrough");
    }

    public bool Available { get; private set; }

    public bool EnabledPassthrough
    {
        get => enabledPassthrough;
        set
        {
            if (enabledPassthrough != value)
            {
                enabledPassthrough = value;
                if (enabledPassthrough) StartPassthrough();
                if (!enabledPassthrough) EndPassthrough();
            }
        }
    }

    public override bool Initialize()
    {
        Available =
            Backend.XRType == BackendXRType.OpenXR &&
            Backend.OpenXR.ExtEnabled("XR_FB_passthrough") &&
            openXRBindings.LoadOpenXRBindings();

        if (Available)
        {
            EnabledPassthrough = true;
        }

        return true;
    }

    public override void Step()
    {
        if (!EnabledPassthrough) return;

        XrCompositionLayerPassthroughFB layer = new(
            XrCompositionLayerFlags.BLEND_TEXTURE_SOURCE_ALPHA_BIT, activeLayer);
        Backend.OpenXR.AddCompositionLayer(layer, -1);

    }

    public override void Shutdown()
    {
        EnabledPassthrough = false;
    }

    private void StartPassthrough()
    {
        if (!Available) return;
        if (passthroughRunning) return;
        passthroughRunning = true;

        oldColor = Renderer.ClearColor;
        oldSky = Renderer.EnableSky;

        XrResult result = openXRBindings.xrCreatePassthroughFB(
            Backend.OpenXR.Session,
            new XrPassthroughCreateInfoFB(XrPassthroughFlagsFB.IS_RUNNING_AT_CREATION_BIT_FB),
            out activePassthrough);

        result = openXRBindings.xrCreatePassthroughLayerFB(
            Backend.OpenXR.Session,
            new XrPassthroughLayerCreateInfoFB(activePassthrough, XrPassthroughFlagsFB.IS_RUNNING_AT_CREATION_BIT_FB,
                XrPassthroughLayerPurposeFB.RECONSTRUCTION_FB),
            out activeLayer);

        Renderer.ClearColor = Color.BlackTransparent;
        Renderer.EnableSky = false;
    }

    private void EndPassthrough()
    {
        if (!passthroughRunning) return;
        passthroughRunning = false;

        openXRBindings.xrPassthroughPauseFB(activePassthrough);
        openXRBindings.xrDestroyPassthroughLayerFB(activeLayer);
        openXRBindings.xrDestroyPassthroughFB(activePassthrough);

        Renderer.ClearColor = oldColor;
        Renderer.EnableSky = oldSky;
    }
}