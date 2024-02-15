using System.Linq;
using UnityEditor.XR.Management;
using UnityEngine;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Features.Meta;
using UnityEngine.XR.OpenXR.Features.MetaQuestSupport;

namespace UnityEditor.XR.OpenXR.Features.Meta
{
    static class XRManagerEditorUtility
    {
        static readonly string[] k_RequiredFeatureIds = { ARSessionFeature.featureId, MetaQuestFeature.featureId };
        static XRGeneralSettingsPerBuildTarget s_AllSettings;

        internal static bool IsMetaOpenXRTheActiveBuildTarget()
        {
            foreach (var openXRFeature in FeatureHelpers.GetFeaturesWithIdsForBuildTarget(BuildTargetGroup.Android, k_RequiredFeatureIds))
            {
                if (!openXRFeature.enabled)
                    return false;
            }

            return IsLoaderActiveForBuildTarget<OpenXRLoader>(BuildTargetGroup.Android) &&
                EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;
        }

        internal static bool IsLoaderActiveForBuildTarget<TLoader>(BuildTargetGroup buildTarget)
        {
            if (s_AllSettings == null && !TryLoadSettings(out s_AllSettings))
                return false;

            if (!s_AllSettings.HasSettingsForBuildTarget(buildTarget))
                return false;

            var settings = s_AllSettings.SettingsForBuildTarget(buildTarget);
            if (settings.Manager == null)
                return false;

            return settings.Manager.activeLoaders?.OfType<TLoader>().Any() ?? false;
        }

        static bool TryLoadSettings(out XRGeneralSettingsPerBuildTarget settings)
        {
            settings = null;

            var assets = AssetDatabase.FindAssets("t:XRGeneralSettingsPerBuildTarget");
            if (assets == null || assets.Length == 0)
                return false;

            if (assets.Length > 1)
            {
                Debug.LogWarning(
                    $"Multiple {nameof(XRGeneralSettingsPerBuildTarget)} assets were found in the project. " +
                    "This is likely a configuration error. Only one instance will be used to check available XRLoaders.");
            }

            settings = AssetDatabase.LoadAssetAtPath<XRGeneralSettingsPerBuildTarget>(AssetDatabase.GUIDToAssetPath(assets[0]));
            return settings != null;
        }
    }
}
