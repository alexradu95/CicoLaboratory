using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Core.Functionalities.Passthrough.OpenXRBindings.Enums;
using static Core.Functionalities.Passthrough.PassthroughOpenXRBindings;

namespace Core.Functionalities.Passthrough.OpenXRBindings.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct XrCompositionLayerPassthroughFB
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
}
