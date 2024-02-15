using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace UnityEngine.XR.OpenXR.Features.Meta
{
    /// <summary>
    /// The Meta-OpenXR implementation of <see cref="XRPlaneSubsystem"/>, built with the Meta OpenXR Mobile SDK.
    /// Planes are provided based on bounded2d components present in your
    /// [Scene Model](https://developer.oculus.com/documentation/native/android/openxr-scene-overview#scene-model).
    /// </summary>
    [Preserve]
    public sealed class MetaOpenXRPlaneSubsystem : XRPlaneSubsystem
    {
        internal const string k_SubsystemId = "MetaOpenXR-Plane";

        const string k_AndroidScenePermission = "com.oculus.permission.USE_SCENE";

        /// <summary>
        /// Plane alignment is determined by calculating the offset of the normal vector components
        /// from 0. For horizontal planes, the x and z vector components are each checked to be
        /// within the threshold and the y vector component is used for vertical planes.
        /// </summary>
        /// <value>The threshold value in meters.</value>
        public static float planeAlignmentThreshold
        {
            get => MetaOpenXRPlaneProvider.planeAlignmentThreshold;
            set => MetaOpenXRPlaneProvider.planeAlignmentThreshold = value;
        }

        class MetaOpenXRPlaneProvider : Provider
        {
            bool m_HasDiscoveredPlanes;

            /// <inheritdoc/>
            public override PlaneDetectionMode requestedPlaneDetectionMode
            {
                get => NativeApi.GetPlaneDetectionMode();
                set => NativeApi.SetPlaneDetectionMode(value);
            }

            /// <inheritdoc/>
            public override PlaneDetectionMode currentPlaneDetectionMode => requestedPlaneDetectionMode;

            /// <inheritdoc/>
            protected override bool TryInitialize()
            {
                if (OpenXRRuntime.IsExtensionEnabled(Constants.OpenXRExtensions.k_XR_FB_spatial_entity) &&
                    OpenXRRuntime.IsExtensionEnabled(Constants.OpenXRExtensions.k_XR_FB_spatial_entity_query) &&
                    OpenXRRuntime.IsExtensionEnabled(Constants.OpenXRExtensions.k_XR_FB_spatial_entity_storage) &&
                    OpenXRRuntime.IsExtensionEnabled(Constants.OpenXRExtensions.k_XR_FB_scene))
                {
                    NativeApi.Create();
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Plane alignment is determined by calculating the offset of the normal vector components
            /// from 0. For horizontal planes, the x and z vector components are each checked to be
            /// within the threshold and the y vector component is used for vertical planes.
            /// </summary>
            /// <value>The threshold value in meters.</value>
            internal static float planeAlignmentThreshold
            {
                get => NativeApi.GetPlaneAlignmentThreshold();
                set => NativeApi.SetPlaneAlignmentThreshold(value);
            }

            /// <inheritdoc/>
            public override void Start()
            {
#if UNITY_ANDROID
                // Meta requires that we ask for scene permission beginning with OpenXR 1.0.31
                if (OpenXRUtility.IsOpenXRVersionGreaterOrEqual(1, 0, 31) &&
                    !Permission.HasUserAuthorizedPermission(k_AndroidScenePermission))
                {
                    var callbacks = new PermissionCallbacks();
                    callbacks.PermissionDenied += _ => LogAndroidPermissionFailure();
#if UNITY_2023_1_OR_NEWER
                    callbacks.PermissionRequestDismissed += _ => LogAndroidPermissionFailure();
#else
                    callbacks.PermissionDeniedAndDontAskAgain += _ => LogAndroidPermissionFailure();
#endif // UNITY_2023_1_OR_NEWER
                    Permission.RequestUserPermission(k_AndroidScenePermission, callbacks);
                }
#endif // UNITY_ANDROID

                NativeApi.Start();
            }

#if UNITY_ANDROID
            static void LogAndroidPermissionFailure() =>
                Debug.LogError($"Plane detection requires system permission {k_AndroidScenePermission}, but permission was not granted.");
#endif

            /// <inheritdoc/>
            public override void Stop() => NativeApi.Stop();

            /// <inheritdoc/>
            public override void Destroy() => NativeApi.Destroy();

            /// <inheritdoc/>
            public override unsafe void GetBoundary(
                TrackableId trackableId,
                Allocator allocator,
                ref NativeArray<Vector2> boundary)
            {
                uint vertexCount = NativeApi.GetBoundaryVertexCount(in trackableId);
                int vertexCountAsInt = (int)vertexCount;

                if (vertexCountAsInt < 0)
                {
                    throw new OverflowException("Exceeded the maximum number of boundary vertices.");
                }

                CreateOrResizeNativeArrayIfNecessary(vertexCountAsInt, allocator, ref boundary);
                NativeApi.GetBoundaryVertexData(in trackableId, boundary.GetUnsafePtr(), vertexCountAsInt);

                FlipBoundaryWindingOrder(boundary);
            }

            static void FlipBoundaryWindingOrder(NativeArray<Vector2> vertices)
            {
                var half = vertices.Length / 2;
                for (var i = 0; i < half; ++i)
                {
                    var j = vertices.Length - 1 - i;
                    (vertices[j], vertices[i]) = (vertices[i], vertices[j]);
                }
            }

            /// <inheritdoc/>
            public override unsafe TrackableChanges<BoundedPlane> GetChanges(BoundedPlane defaultPlane, Allocator allocator)
            {
                NativeApi.GetChanges(
                    out var addedPtr, out var addedCount,
                    out var updatedPtr, out var updatedCount,
                    out var removedPtr, out var removedCount,
                    out var elementSize);

                try
                {
                    return new TrackableChanges<BoundedPlane>(
                        addedPtr, addedCount,
                        updatedPtr, updatedCount,
                        removedPtr, removedCount,
                        defaultPlane, elementSize,
                        allocator);
                }
                finally
                {
                    NativeApi.ReleaseChanges();
                }
            }

            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
            static void RegisterDescriptor()
            {
                XRPlaneSubsystemDescriptor.Create(new XRPlaneSubsystemDescriptor.Cinfo
                {
                    id = k_SubsystemId,
                    providerType = typeof(MetaOpenXRPlaneProvider),
                    subsystemTypeOverride = typeof(MetaOpenXRPlaneSubsystem),
                    supportsHorizontalPlaneDetection = false,
                    supportsVerticalPlaneDetection = false,
                    supportsArbitraryPlaneDetection = true,
                    supportsBoundaryVertices = true,
                    supportsClassification = false
                });
            }

            static class NativeApi
            {
                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRMeta_Plane_Create")]
                public static extern void Create();

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRMeta_Plane_Start")]
                public static extern void Start();

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRMeta_Plane_Stop")]
                public static extern void Stop();

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRMeta_Plane_Destroy")]
                public static extern void Destroy();

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRMeta_Plane_GetChanges")]
                public static extern unsafe void GetChanges(
                    out void* addedPtr, out int addedCount,
                    out void* updatedPtr, out int updatedCount,
                    out void* removedPtr, out int removedCount,
                    out int elementSize);

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRMeta_Plane_SetPlaneAlignmentThreshold")]
                public static extern void SetPlaneAlignmentThreshold(float epsilon);

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRMeta_Plane_GetPlaneAlignmentThreshold")]
                public static extern float GetPlaneAlignmentThreshold();

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRMeta_Plane_SetPlaneDetectionMode")]
                public static extern void SetPlaneDetectionMode(PlaneDetectionMode planeDetectionMode);

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRMeta_Plane_GetPlaneDetectionMode")]
                public static extern PlaneDetectionMode GetPlaneDetectionMode();

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRMeta_Plane_GetBoundaryVertexCount")]
                public static extern uint GetBoundaryVertexCount(in TrackableId trackableId);

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRMeta_Plane_GetBoundaryVertexData")]
                public static extern unsafe void GetBoundaryVertexData(in TrackableId trackableId, void* boundary, int numVertices);

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRMeta_Plane_ReleaseChanges")]
                public static extern void ReleaseChanges();
            }
        }
    }
}
