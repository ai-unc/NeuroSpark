using System.Runtime.InteropServices;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.OpenXR.Features.Meta
{
    /// <summary>
    /// The Meta-OpenXR implementation of the <see cref="XRSessionSubsystem"/>.
    /// Do not create this directly. Use the <see cref="SubsystemManager"/> instead.
    /// </summary>
    public sealed class MetaOpenXRSessionSubsystem : XRSessionSubsystem
    {
        internal const string k_SubsystemId = "Meta-Session";

        internal static MetaOpenXRSessionSubsystem instance { get; private set; }

        /// <summary>
        /// Do not call this directly. Call create on a valid <see cref="XRSessionSubsystemDescriptor"/> instead.
        /// </summary>
        public MetaOpenXRSessionSubsystem()
        {
            instance = this;
        }

        /// <summary>
        /// Attempts to initiate Meta [scene capture](xref:meta-openxr-session#scene-capture).
        /// </summary>
        /// <returns><see langword="true"/> if the request was successful. Otherwise, <see langword="false"/>.</returns>
        public bool TryRequestSceneCapture()
        {
            var success = MetaOpenXRProvider.TryRequestSceneCapture();

            if (!success)
            {
                Debug.LogError("Failed to request scene capture.");
            }

            return success;
        }

        internal void OnSessionStateChange(int oldState, int newState)
            => ((MetaOpenXRProvider)provider).OnSessionStateChange(oldState, newState);

        class MetaOpenXRProvider : Provider
        {
            XrSessionState m_SessionState;

            /// <inheritdoc/>
            public override TrackingState trackingState
            {
                get
                {
                    switch (m_SessionState)
                    {
                        case XrSessionState.Idle:
                        case XrSessionState.Ready:
                        case XrSessionState.Synchronized:
                            return TrackingState.Limited;

                        case XrSessionState.Visible:
                        case XrSessionState.Focused:
                            return TrackingState.Tracking;

                        case XrSessionState.Unknown:
                        case XrSessionState.Stopping:
                        case XrSessionState.LossPending:
                        case XrSessionState.Exiting:
                        default:
                            return TrackingState.None;
                    }
                }
            }

            /// <inheritdoc/>
            public override NotTrackingReason notTrackingReason
            {
                get
                {
                    switch (m_SessionState)
                    {
                        case XrSessionState.Idle:
                        case XrSessionState.Ready:
                        case XrSessionState.Synchronized:
                            return NotTrackingReason.Initializing;

                        case XrSessionState.Visible:
                        case XrSessionState.Focused:
                            return NotTrackingReason.None;

                        case XrSessionState.Unknown:
                        case XrSessionState.Stopping:
                        case XrSessionState.LossPending:
                        case XrSessionState.Exiting:
                        default:
                            return NotTrackingReason.Unsupported;
                    }
                }
            }

            /// <inheritdoc/>
            public MetaOpenXRProvider() => NativeApi.UnityOpenXRMeta_Session_Construct();

            /// <inheritdoc/>
            public override void Start() => NativeApi.UnityOpenXRMeta_Session_Start();

            /// <inheritdoc/>
            public override void Stop() => NativeApi.UnityOpenXRMeta_Session_Stop();

            /// <inheritdoc/>
            public override void Destroy() => NativeApi.UnityOpenXRMeta_Session_Destruct();

            /// <inheritdoc/>
            public override Promise<SessionAvailability> GetAvailabilityAsync()
                => Promise<SessionAvailability>.CreateResolvedPromise(
                    NativeApi.UnityOpenXRMeta_Session_IsSupported()
                        ? SessionAvailability.Supported | SessionAvailability.Installed
                        : SessionAvailability.None);

            public void OnSessionStateChange(int oldState, int newState)
            {
                m_SessionState = (XrSessionState)newState;
            }

            internal static bool TryRequestSceneCapture() => NativeApi.TryRequestSceneCapture();
        }

        enum XrSessionState
        {
            Unknown = 0,
            Idle = 1,
            Ready = 2,
            Synchronized = 3,
            Visible = 4,
            Focused = 5,
            Stopping = 6,
            LossPending = 7,
            Exiting = 8,
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
            XRSessionSubsystemDescriptor.RegisterDescriptor(new XRSessionSubsystemDescriptor.Cinfo
            {
                id = k_SubsystemId,
                providerType = typeof(MetaOpenXRProvider),
                subsystemTypeOverride = typeof(MetaOpenXRSessionSubsystem),
                supportsInstall = false,
                supportsMatchFrameRate = false
            });
        }

        static class NativeApi
        {
            [DllImport(Constants.k_ARFoundationLibrary)]
            public static extern void UnityOpenXRMeta_Session_Construct();

            [DllImport(Constants.k_ARFoundationLibrary)]
            public static extern void UnityOpenXRMeta_Session_Destruct();

            [DllImport(Constants.k_ARFoundationLibrary)]
            public static extern void UnityOpenXRMeta_Session_Start();

            [DllImport(Constants.k_ARFoundationLibrary)]
            public static extern void UnityOpenXRMeta_Session_Stop();

            [DllImport(Constants.k_ARFoundationLibrary)]
            public static extern bool UnityOpenXRMeta_Session_IsSupported();

            [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRMeta_SceneCapture_TryRequestSceneCapture")]
            public static extern bool TryRequestSceneCapture();
        }
    }
}
