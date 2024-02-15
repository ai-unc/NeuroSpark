using System;
using System.Runtime.InteropServices;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.OpenXR.Features.Meta
{
    /// <summary>
    /// The Meta-OpenXR implementation of the <see cref="XRCameraSubsystem"/>.
    /// Do not create this directly. Use the <see cref="SubsystemManager"/> instead.
    /// </summary>
    public sealed class MetaOpenXRCameraSubsystem : XRCameraSubsystem
    {
        internal const string k_SubsystemId = "Meta-Camera";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
            var cameraSubsystemCinfo = new XRCameraSubsystemCinfo
            {
                id = k_SubsystemId,
                providerType = typeof(MetaOpenXRProvider),
                subsystemTypeOverride = typeof(MetaOpenXRCameraSubsystem),
                supportsAverageBrightness = false,
                supportsAverageColorTemperature = false,
                supportsColorCorrection = false,
                supportsDisplayMatrix = false,
                supportsProjectionMatrix = false,
                supportsTimestamp = false,
                supportsCameraConfigurations = false,
                supportsCameraImage = false,
                supportsAverageIntensityInLumens = false,
                supportsFocusModes = false,
                supportsFaceTrackingAmbientIntensityLightEstimation = false,
                supportsFaceTrackingHDRLightEstimation = false,
                supportsWorldTrackingAmbientIntensityLightEstimation = false,
                supportsWorldTrackingHDRLightEstimation = false,
                supportsCameraGrain = false,
            };

            if (!XRCameraSubsystem.Register(cameraSubsystemCinfo))
            {
                Debug.LogError($"Failed to register the {k_SubsystemId} subsystem.");
            }
        }

        class MetaOpenXRProvider : Provider
        {
            protected override bool TryInitialize()
            {
                if (OpenXRRuntime.IsExtensionEnabled(Constants.OpenXRExtensions.k_XR_FB_passthrough) &&
                    OpenXRRuntime.IsExtensionEnabled(Constants.OpenXRExtensions.k_XR_FB_composition_layer_alpha_blend))
                {
                    NativeApi.UnityMetaQuest_Passthrough_Construct();
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Start the camera functionality.
            /// </summary>
            public override void Start() => NativeApi.UnityMetaQuest_Passthrough_Start();

            /// <summary>
            /// Stop the camera functionality.
            /// </summary>
            public override void Stop() => NativeApi.UnityMetaQuest_Passthrough_Stop();

            /// <summary>
            /// Destroy any resources required for the camera functionality.
            /// </summary>
            public override void Destroy() => NativeApi.UnityMetaQuest_Passthrough_Destruct();

            /// <summary>
            /// Container to wrap the native Meta OpenXR camera APIs.
            /// </summary>
            static class NativeApi
            {
                [DllImport(Constants.k_ARFoundationLibrary)]
                public static extern void UnityMetaQuest_Passthrough_Construct();

                [DllImport(Constants.k_ARFoundationLibrary)]
                public static extern void UnityMetaQuest_Passthrough_Destruct();

                [DllImport(Constants.k_ARFoundationLibrary)]
                public static extern void UnityMetaQuest_Passthrough_Start();

                [DllImport(Constants.k_ARFoundationLibrary)]
                public static extern void UnityMetaQuest_Passthrough_Stop();
            }
        }
    }
}
