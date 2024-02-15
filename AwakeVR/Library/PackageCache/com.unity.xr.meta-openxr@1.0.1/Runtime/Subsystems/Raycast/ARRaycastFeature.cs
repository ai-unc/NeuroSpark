using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.XR.OpenXR.Features;
#endif

namespace UnityEngine.XR.OpenXR.Features.Meta
{
    /// <summary>
    /// Enables AR Foundation raycast support via OpenXR for Meta Quest devices.
    /// </summary>
#if UNITY_EDITOR
    [OpenXRFeature(UiName = "Meta Quest: AR Raycasts",
        BuildTargetGroups = new[] { BuildTargetGroup.Android },
        Company = Constants.k_CompanyName,
        Desc = "AR Foundation raycast support on Meta Quest devices",
        DocumentationLink = Constants.DocsUrls.k_RaycastsUrl,
        OpenxrExtensionStrings = "",
        Category = FeatureCategory.Feature,
        FeatureId = featureId,
        Version = "0.1.0")]
#endif
    public class ARRaycastFeature : OpenXRFeature
    {
        /// <summary>
        /// The feature id string. This is used to give the feature a well known id for reference.
        /// </summary>
        public const string featureId = "com.unity.openxr.feature.arfoundation-meta-raycast";

        static List<XRRaycastSubsystemDescriptor> s_RaycastDescriptors = new();

        /// <summary>
        /// Instantiates Meta OpenXR raycast subsystem instances, but does not start it.
        /// (Start/Stop is typically handled by AR Foundation managers.)
        /// </summary>
        protected override void OnSubsystemCreate()
        {
            CreateSubsystem<XRRaycastSubsystemDescriptor, XRRaycastSubsystem>(
                s_RaycastDescriptors,
                MetaOpenXRRaycastSubsystem.k_SubsystemId);
        }

        /// <summary>
        /// Destroys the raycast subsystem.
        /// </summary>
        protected override void OnSubsystemDestroy()
        {
            DestroySubsystem<XRRaycastSubsystem>();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Validation Rules for ARRaycastFeature.
        /// </summary>
        protected override void GetValidationChecks(List<ValidationRule> rules, BuildTargetGroup targetGroup)
        {
            rules.AddRange(SharedValidationRules.EnableARSessionValidationRules(this));
        }
#endif
    }
}
