#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.XR.ARSubsystems;
using UnityEditor;

namespace UnityEngine.XR.OpenXR.Features.Meta
{
    /// <summary>
    /// Shared validation rules for different OpenXRFeatures.
    /// </summary>
    static class SharedValidationRules
    {
        internal static OpenXRFeature.ValidationRule[] EnableARSessionValidationRules(OpenXRFeature feature) => new OpenXRFeature.ValidationRule[]
        {
            new OpenXRFeature.ValidationRule(feature)
            {
                message = "Meta Quest ARSession feature must be enabled for this AR feature.",
                checkPredicate = () =>
                {
                    OpenXRSettings androidOpenXRSettings = OpenXRSettings.GetSettingsForBuildTargetGroup(BuildTargetGroup.Android);
                    var arsessionFeature = androidOpenXRSettings.GetFeature<ARSessionFeature>();
                    return (arsessionFeature != null && arsessionFeature.enabled) ? true : false;
                },
                fixItAutomatic = true,
                fixItMessage = "Open Project Settings > XR Plug-in Management > OpenXR > Android tab. In the list of 'OpenXR Feature Groups', " +
                                "make sure 'AR Foundation: Meta Quest Session' is checked.",
                fixIt = () =>
                {
                    OpenXRSettings androidOpenXRSettings = OpenXRSettings.GetSettingsForBuildTargetGroup(BuildTargetGroup.Android);
                    var arsessionFeature = androidOpenXRSettings.GetFeature<ARSessionFeature>();
                    if (arsessionFeature != null)
                    {
                        arsessionFeature.enabled = true;
                    }
                },
                error = false
            }
        };
    }
}
#endif
