using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.OpenXR.Features.Meta
{
    /// <summary>
    /// Meta-OpenXR implementation of <see cref="XRRaycastSubsystem"/>. This implementation does not
    /// perform provider-based raycast, but is implemented to allow
    /// [ARRaycastManager](xref:UnityEngine.XR.ARFoundation.ARRaycastManager) to execute the fallback path.
    /// </summary>
    public sealed class MetaOpenXRRaycastSubsystem : XRRaycastSubsystem
    {
        internal const string k_SubsystemId = "Meta-Raycast";

        class MetaOpenXRRaycastProvider : Provider
        {
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
            XRRaycastSubsystemDescriptor.RegisterDescriptor(new XRRaycastSubsystemDescriptor.Cinfo
            {
                id = k_SubsystemId,
                providerType = typeof(MetaOpenXRRaycastProvider),
                subsystemTypeOverride = typeof(MetaOpenXRRaycastSubsystem),
                supportsViewportBasedRaycast = false,
                supportsWorldBasedRaycast = false,
                supportedTrackableTypes = TrackableType.PlaneWithinBounds,
                supportsTrackedRaycasts = false,
            });
        }
    }
}
