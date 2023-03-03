using System.Runtime.InteropServices;
using Nazar.Framework;
using StereoKit;

namespace Nazar.Core.Passthrough;

public class PassthroughCore : Node
{
    private XrPassthroughLayerFB activeLayer;
    private XrPassthroughFB activePassthrough;
    private bool enabledPassthrough;

    private Color oldColor;
    private bool oldSky;
    private bool passthroughRunning;

    public PassthroughCore()
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

    public override bool Enabled => throw new System.NotImplementedException();

    new public void Shutdown()
    {
        EnabledPassthrough = false;
        base.Shutdown();
    }


    override public bool Initialize()
    {
        Available =
            Backend.XRType == BackendXRType.OpenXR &&
            Backend.OpenXR.ExtEnabled("XR_FB_passthrough") &&
            LoadBindings();

        if (Available)
        {
            EnabledPassthrough = true;
        }


        return true;
    }

    public override void Step()
    {
        base.Step();
        if (!EnabledPassthrough) return;

        XrCompositionLayerPassthroughFB layer = new(
            XrCompositionLayerFlags.BLEND_TEXTURE_SOURCE_ALPHA_BIT, activeLayer);
        Backend.OpenXR.AddCompositionLayer(layer, -1);
    }

    private void StartPassthrough()
    {
        if (!Available) return;
        if (passthroughRunning) return;
        passthroughRunning = true;

        oldColor = Renderer.ClearColor;
        oldSky = Renderer.EnableSky;

        XrResult result = xrCreatePassthroughFB(
            Backend.OpenXR.Session,
            new XrPassthroughCreateInfoFB(XrPassthroughFlagsFB.IS_RUNNING_AT_CREATION_BIT_FB),
            out activePassthrough);

        result = xrCreatePassthroughLayerFB(
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

        xrPassthroughPauseFB(activePassthrough);
        xrDestroyPassthroughLayerFB(activeLayer);
        xrDestroyPassthroughFB(activePassthrough);

        Renderer.ClearColor = oldColor;
        Renderer.EnableSky = oldSky;
    }

    #region OpenXR native bindings and types

    private enum XrStructureType : ulong
    {
        XR_TYPE_PASSTHROUGH_CREATE_INFO_FB = 1000118001,
        XR_TYPE_PASSTHROUGH_LAYER_CREATE_INFO_FB = 1000118002,
        XR_TYPE_PASSTHROUGH_STYLE_FB = 1000118020,
        XR_TYPE_COMPOSITION_LAYER_PASSTHROUGH_FB = 1000118003
    }

    private enum XrPassthroughFlagsFB : ulong
    {
        None = 0,
        IS_RUNNING_AT_CREATION_BIT_FB = 0x00000001
    }

    private enum XrCompositionLayerFlags : ulong
    {
        None = 0,
        CORRECT_CHROMATIC_ABERRATION_BIT = 0x00000001,
        BLEND_TEXTURE_SOURCE_ALPHA_BIT = 0x00000002,
        UNPREMULTIPLIED_ALPHA_BIT = 0x00000004
    }

    private enum XrPassthroughLayerPurposeFB : uint
    {
        RECONSTRUCTION_FB = 0,
        PROJECTED_FB = 1,
        TRACKED_KEYBOARD_HANDS_FB = 1000203001,
        MAX_ENUM_FB = 0x7FFFFFFF
    }

    private enum XrResult : uint
    {
        Success = 0
    }

#pragma warning disable 0169 // handle is not "used", but required for interop
    private struct XrPassthroughFB
    {
        private ulong handle;
    }

    private struct XrPassthroughLayerFB
    {
        private ulong handle;
    }
#pragma warning restore 0169

    [StructLayout(LayoutKind.Sequential)]
    private struct XrPassthroughCreateInfoFB
    {
        private readonly XrStructureType type;
        public readonly nint next;
        public readonly XrPassthroughFlagsFB flags;

        public XrPassthroughCreateInfoFB(XrPassthroughFlagsFB passthroughFlags)
        {
            type = XrStructureType.XR_TYPE_PASSTHROUGH_CREATE_INFO_FB;
            next = nint.Zero;
            flags = passthroughFlags;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct XrPassthroughLayerCreateInfoFB
    {
        private readonly XrStructureType type;
        public readonly nint next;
        public readonly XrPassthroughFB passthrough;
        public readonly XrPassthroughFlagsFB flags;
        public readonly XrPassthroughLayerPurposeFB purpose;

        public XrPassthroughLayerCreateInfoFB(XrPassthroughFB passthrough, XrPassthroughFlagsFB flags,
            XrPassthroughLayerPurposeFB purpose)
        {
            type = XrStructureType.XR_TYPE_PASSTHROUGH_LAYER_CREATE_INFO_FB;
            next = nint.Zero;
            this.passthrough = passthrough;
            this.flags = flags;
            this.purpose = purpose;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct XrPassthroughStyleFB
    {
        public readonly XrStructureType type;
        public readonly nint next;
        public readonly float textureOpacityFactor;
        public readonly Color edgeColor;

        public XrPassthroughStyleFB(float textureOpacityFactor, Color edgeColor)
        {
            type = XrStructureType.XR_TYPE_PASSTHROUGH_STYLE_FB;
            next = nint.Zero;
            this.textureOpacityFactor = textureOpacityFactor;
            this.edgeColor = edgeColor;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct XrCompositionLayerPassthroughFB
    {
        public readonly XrStructureType type;
        public readonly nint next;
        public readonly XrCompositionLayerFlags flags;
        public readonly ulong space;
        public readonly XrPassthroughLayerFB layerHandle;

        public XrCompositionLayerPassthroughFB(XrCompositionLayerFlags flags, XrPassthroughLayerFB layerHandle)
        {
            type = XrStructureType.XR_TYPE_COMPOSITION_LAYER_PASSTHROUGH_FB;
            next = nint.Zero;
            space = 0;
            this.flags = flags;
            this.layerHandle = layerHandle;
        }
    }

    private delegate XrResult del_xrCreatePassthroughFB(ulong session, [In] XrPassthroughCreateInfoFB createInfo,
        out XrPassthroughFB outPassthrough);

    private delegate XrResult del_xrDestroyPassthroughFB(XrPassthroughFB passthrough);

    private delegate XrResult del_xrPassthroughStartFB(XrPassthroughFB passthrough);

    private delegate XrResult del_xrPassthroughPauseFB(XrPassthroughFB passthrough);

    private delegate XrResult del_xrCreatePassthroughLayerFB(ulong session,
        [In] XrPassthroughLayerCreateInfoFB createInfo, out XrPassthroughLayerFB outLayer);

    private delegate XrResult del_xrDestroyPassthroughLayerFB(XrPassthroughLayerFB layer);

    private delegate XrResult del_xrPassthroughLayerPauseFB(XrPassthroughLayerFB layer);

    private delegate XrResult del_xrPassthroughLayerResumeFB(XrPassthroughLayerFB layer);

    private delegate XrResult del_xrPassthroughLayerSetStyleFB(XrPassthroughLayerFB layer,
        [In] XrPassthroughStyleFB style);

    private del_xrCreatePassthroughFB xrCreatePassthroughFB;
    private del_xrDestroyPassthroughFB xrDestroyPassthroughFB;
    private del_xrPassthroughStartFB xrPassthroughStartFB;
    private del_xrPassthroughPauseFB xrPassthroughPauseFB;
    private del_xrCreatePassthroughLayerFB xrCreatePassthroughLayerFB;
    private del_xrDestroyPassthroughLayerFB xrDestroyPassthroughLayerFB;
    private del_xrPassthroughLayerPauseFB xrPassthroughLayerPauseFB;
    private del_xrPassthroughLayerResumeFB xrPassthroughLayerResumeFB;
    private del_xrPassthroughLayerSetStyleFB xrPassthroughLayerSetStyleFB;

    private bool LoadBindings()
    {
        xrCreatePassthroughFB = Backend.OpenXR.GetFunction<del_xrCreatePassthroughFB>("xrCreatePassthroughFB");
        xrDestroyPassthroughFB = Backend.OpenXR.GetFunction<del_xrDestroyPassthroughFB>("xrDestroyPassthroughFB");
        xrPassthroughStartFB = Backend.OpenXR.GetFunction<del_xrPassthroughStartFB>("xrPassthroughStartFB");
        xrPassthroughPauseFB = Backend.OpenXR.GetFunction<del_xrPassthroughPauseFB>("xrPassthroughPauseFB");
        xrCreatePassthroughLayerFB =
            Backend.OpenXR.GetFunction<del_xrCreatePassthroughLayerFB>("xrCreatePassthroughLayerFB");
        xrDestroyPassthroughLayerFB =
            Backend.OpenXR.GetFunction<del_xrDestroyPassthroughLayerFB>("xrDestroyPassthroughLayerFB");
        xrPassthroughLayerPauseFB =
            Backend.OpenXR.GetFunction<del_xrPassthroughLayerPauseFB>("xrPassthroughLayerPauseFB");
        xrPassthroughLayerResumeFB =
            Backend.OpenXR.GetFunction<del_xrPassthroughLayerResumeFB>("xrPassthroughLayerResumeFB");
        xrPassthroughLayerSetStyleFB =
            Backend.OpenXR.GetFunction<del_xrPassthroughLayerSetStyleFB>("xrPassthroughLayerSetStyleFB");

        return
            xrCreatePassthroughFB != null &&
            xrDestroyPassthroughFB != null &&
            xrPassthroughStartFB != null &&
            xrPassthroughPauseFB != null &&
            xrCreatePassthroughLayerFB != null &&
            xrDestroyPassthroughLayerFB != null &&
            xrPassthroughLayerPauseFB != null &&
            xrPassthroughLayerResumeFB != null &&
            xrPassthroughLayerSetStyleFB != null;
    }

    #endregion
}