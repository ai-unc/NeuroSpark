using System.Runtime.CompilerServices;

namespace UnityEngine.XR.OpenXR.Features.Meta
{
    static class OpenXRUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsOpenXRFeatureEnabled<T>() where T : OpenXRFeature
        {
            var feature = OpenXRSettings.Instance.GetFeature<T>();
            return feature != null && feature.enabled;
        }

        /// <remarks>
        /// The OpenXR version reported by OpenXRRuntime is the version requested by the OpenXR Plug-in during
        /// `xrCreateInstance`. The actual version that you are linked against may be less than this depending on
        /// which version of OpenXR was used to build the device's Meta Quest Software version.
        ///
        /// The OpenXR specification has no way for us to access the actual version that we are linked against, however.
        /// So the best we can do is check against the version that the OpenXR Plug-in requested.
        /// </remarks>
        internal static bool IsOpenXRVersionGreaterOrEqual(int major, int minor, int patch)
        {
            var split = OpenXRRuntime.apiVersion.Split('.');
            if (split.Length != 3)
            {
                Debug.LogError($"Invalid OpenXR version number: {OpenXRRuntime.apiVersion}");
                return false;
            }
            if (!int.TryParse(split[0], out var majorValue))
            {
                Debug.LogError($"Invalid OpenXR version number: {OpenXRRuntime.apiVersion}");
                return false;
            }
            if (!int.TryParse(split[1], out var minorValue))
            {
                Debug.LogError($"Invalid OpenXR version number: {OpenXRRuntime.apiVersion}");
                return false;
            }
            if (!int.TryParse(split[2], out var patchValue))
            {
                Debug.LogError($"Invalid OpenXR version number: {OpenXRRuntime.apiVersion}");
                return false;
            }

            return majorValue > major ||
                (majorValue == major && minorValue > minor) ||
                (majorValue == major && minorValue == minor && patchValue >= patch);
        }
    }
}
